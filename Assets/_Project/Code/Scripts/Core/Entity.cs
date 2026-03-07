using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public abstract class Entity : MonoBehaviour, IDamagable, IMoveable
{
    public EntityData Data;

    private Coroutine _weightCoroutine;

    protected Animator anim;
    protected Transform currentTarget;

    [Header("Sockets")]
    public Transform WeaponSocket;
    public Transform ShieldSocket;

    [Header("Sensing Settings")]
    [SerializeField] private float searchInterval = 0.2f;
    private float _searchTimer;
    private readonly float EyeHeight = 0.5f;
    [Tooltip("The number of enemies the player can detect as potential targets at the same time")]
    private static readonly int MaxDetectionBuffer = 32;
    private readonly Collider[] _targetsBuffer = new Collider[MaxDetectionBuffer];

    [Header("Regeneration Settings")]
    [SerializeField] private float regenerationInterval = 1.0f;
    [Tooltip("Time before regeneration kicks in")]
    [SerializeField] private float regenerationDelay = 2.0f;
    private Coroutine _regenCoroutine;

    protected bool _inCombat = false;
    protected bool _isDead = false;

    [Header("Death Arrangements")]
    [Tooltip("Time before the body goes to a better place")]
    [SerializeField] private Material _ghostMaterial;
    [SerializeField] private float restInPeace = 2.0f;
    [SerializeField] private float timeToAscend = 1.5f;
    [SerializeField] private float ascensionHeight = 1.0f;
    public static event Action<Entity> OnAnyEntityDeath;

    [Header("Objective Settings")]
    public bool isMarked = false;

    public float MaxHealth { get; set; }
    private float _currentHealth;
    public float CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = Mathf.Clamp(value, 0.0f, MaxHealth);
    }

    //TODO: Create an interface for casters?
    public float CurrentMana { get; set; }

    public float CurrentStamina { get; set; }

    private float _lastAttackTime;
    private Weapon _currentWeapon;

    public Rigidbody RB { get; set; }
    public bool IsFacingRight { get; set; } = true;  // based on the default gameobject facing direction

    private RigBuilder _rigBuilder;

    #region StateMachine Variables
    public EntityStateMachine StateMachine { get; protected set; }
    public IdleState IdleState { get; protected set; }
    public MoveState MoveState { get; protected set; }
    public AttackState AttackState { get; protected set; }
    #endregion

    protected virtual void Awake()
    {
        RB = GetComponent<Rigidbody>();
        RB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;  // Prevent entities from face palming while trying to walk, they are better than that

        anim = GetComponentInChildren<Animator>();
        _rigBuilder = GetComponentInChildren<RigBuilder>();

        StateMachine = new EntityStateMachine();

        MaxHealth = Data.MaxHealth;
        _currentHealth = MaxHealth;
        CurrentMana = Data.MaxMana;
        CurrentStamina = Data.MaxStamina;

        InitializeEquipment();
    }

    protected virtual void Update()
    {
        _searchTimer -= Time.deltaTime;
        if (_searchTimer <= 0)
        {
            SearchForTarget();
            _searchTimer = searchInterval;
        }

        if (_inCombat)
        {
            if (currentTarget == null || !IsTargetAlive(currentTarget))
            {
                ResetCombatState();
            }
        }
    }

    public void SetRigActive(bool active)
    {
        if (_rigBuilder != null)
        {
            _rigBuilder.enabled = active;
        }
    }
    public abstract Vector2 GetMoveInput();
    public abstract bool IsAttacking();
    public abstract bool IsSprinting();

    #region Combat
    private void SearchForTarget()
    {
        int numFound= Physics.OverlapSphereNonAlloc(transform.position, Data.DetectionRange, _targetsBuffer, Data.EnemyLayer);

        if (numFound == 0)
        {
            currentTarget = null;
            if (_inCombat)
            { 
                ResetCombatState(); 
            }

            return;
        }

        currentTarget = GetBestValidTarget(numFound);

        if (currentTarget != null)
        {
            EnterCombat();
        }
        else if (_inCombat)
        {
            ResetCombatState();
        }
    }

    private Transform GetBestValidTarget(int count)
    {
        float closestDistanceSqr = Mathf.Infinity;
        Transform bestTarget = null;
        Vector3 eyePos = transform.position + (Vector3.up * EyeHeight);

        for (int i = 0; i < count; i++)
        {
            Collider col = _targetsBuffer[i];
            if (col.transform == transform)
            {
                continue;
            }

            if (IsValidTarget(col, eyePos, out float distSqr))
            {
                if (distSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distSqr;
                    bestTarget = col.transform;
                }
            }
        }

        return bestTarget;
    }

    private bool IsValidTarget(Collider col, Vector3 eyePos, out float distSqr)
    {
        distSqr = (col.transform.position - transform.position).sqrMagnitude;

        float maxRangeSqr = Data.DetectionRange * Data.DetectionRange;

        // To reduce the CPU usage with calculating the Square unnecessarily
        if (distSqr > maxRangeSqr)
        {
            return false;
        }


        if (col.TryGetComponent<Entity>(out var entity) && entity.IsDead())
        {
            return false;
        }

        Vector3 targetCenter = col.bounds.center;
        Vector3 dir = (targetCenter - eyePos).normalized;
        float dist = Mathf.Sqrt(distSqr);

        if (Physics.Raycast(eyePos, dir, dist, Data.ObstacleLayer))
        {
            return false;
        }

        return true;
    }

    private bool IsTargetAlive(Transform target)
    {
        if (target.TryGetComponent<Entity>(out var e))
        {
            return e.CurrentHealth > 0;
        }
        return false;
    }

    private void ResetCombatState()
    {
        currentTarget = null;
        _inCombat = false;
        ExitCombat();
    }

    public bool CanAttack()
    {
        return Time.time >= _lastAttackTime + Data.AttackCooldown;
    }

    public Weapon GetCurrentWeapon()
    {
        return _currentWeapon;
    }

    public void ResetAttackCooldown()
    {
        _lastAttackTime = Time.time;
    }

    public void EnterCombat()
    {
        _inCombat = true;

        if (_regenCoroutine != null)
        {
            StopCoroutine(_regenCoroutine);
            _regenCoroutine = null;
        }
    }

    public void ExitCombat()
    {
        if (_regenCoroutine == null && gameObject.activeInHierarchy)
        {
            _regenCoroutine = StartCoroutine(RegenerationHeartbeat());
        }
    }
    #endregion

    #region Regeneration

    private IEnumerator RegenerationHeartbeat()
    {
        yield return new WaitForSeconds(regenerationDelay);

        while (!_inCombat && CurrentHealth > 0.0f)
        {
            Regeneration();

            yield return new WaitForSeconds(regenerationInterval);

            if (CurrentHealth >= MaxHealth && CurrentMana >= Data.MaxMana && CurrentStamina >= Data.MaxStamina)
            {
                _regenCoroutine = null;
                yield break;
            }
        }
        _regenCoroutine = null;
    }

    public void Regeneration()
    {
        if (!IsSprinting())
        {
            CurrentStamina = Mathf.Min(CurrentStamina + Data.StaminaRecoveryRate, Data.MaxStamina);
        }

        CurrentHealth += Data.HealthRegenerationRate;
        CurrentMana = Mathf.Min(CurrentMana + Data.ManaRecoveryRate, Data.MaxMana);
    }

    #endregion

    #region Equipment SetUp
    private void InitializeEquipment()
    {
        if (Data.StartingWeapon != null && WeaponSocket != null)
        {
            EquipItem(Data.StartingWeapon, WeaponSocket);
        }

        if (Data.StartingShield != null && ShieldSocket != null)
        {
            EquipItem(Data.StartingShield, ShieldSocket);
        }
        RegisterEquipment();
    }

    private void EquipItem(EquipmentData data, Transform socket)
    {
        if (data.ModelPrefab == null)
        {
            return;
        }

        GameObject item = Instantiate(data.ModelPrefab, socket);

        if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }

    private void RegisterEquipment()
    {
        RegisterWeapon();

        RegisterShield();
    }

    private void RegisterWeapon()
    {
        _currentWeapon = WeaponSocket.GetComponentInChildren<Weapon>();
        Weapon arm = WeaponSocket.GetComponentInParent<Weapon>();
        arm.DisableWeapon();

        if (_currentWeapon == null)
        {
            _currentWeapon = WeaponSocket.GetComponentInParent<Weapon>();
            arm.EnableWeapon();
        }

        _currentWeapon.Initialize(Data.AttackDamage, Data.EnemyLayer);
    }

    private void RegisterShield()
    {
        //TODO: handle shields - will reduce the amount of damage by the armor value (It can also be procentage of damage reduction)
    }

    #endregion

    #region Health / Die Functions

    public void Damage(float damageAmount)
    {
        EnterCombat();

        CurrentHealth -= damageAmount;
        Debug.Log($"I got hit! I have {CurrentHealth} left");

        anim.SetTrigger("Hurt");

        if (CurrentHealth <= 0.0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("I die!");
        _inCombat = false;
        _isDead = true;
        currentTarget = null;

        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Move(Vector3.zero);
            rb.isKinematic = true;
        }

        if (TryGetComponent<Collider>(out Collider collider))
        {
            collider.enabled = false;
        }

        PlayDeathVisuals();
        StartCoroutine(GetRidOfTheBody());

        OnAnyEntityDeath?.Invoke(this);

        OnDeath();
    }

    protected abstract void OnDeath();

    protected IEnumerator GetRidOfTheBody()
    {
        // First step - waits for X seconds
        yield return new WaitForSeconds(restInPeace);

        // Second step - the body rises slowly to the air and becomes invisible and afther Y seconds is destroyed
        float elapsed = 0.0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * ascensionHeight;

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        Material[] ghostInstances = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            ghostInstances[i] = Instantiate(_ghostMaterial);

            Material[] mats = renderers[i].materials;
            System.Array.Resize(ref mats, mats.Length + 1);
            mats[^1] = ghostInstances[i];
            renderers[i].materials = mats;            
        }

        while (elapsed < timeToAscend)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / timeToAscend;

            transform.position = Vector3.Lerp(startPos, endPos, t);

            foreach (var mat in ghostInstances)
            {
                Color c = mat.GetColor("_Color"); ;
                c.a = Mathf.Lerp(1.0f, 0.0f, t);
                mat.SetColor("_Color", c);
            }

            yield return null;
        }

        Destroy(gameObject);
    }

    public bool IsDead() => _isDead;

    #endregion

    #region Movement Functions

    public void Move(Vector3 velocity)
    {
        RB.linearVelocity = velocity;
    }

    public void HandleOrientation(Vector3 moveDirection)
    {
        Quaternion targetRotation = transform.rotation;

        if (currentTarget != null)
        {
            Vector3 dir = (currentTarget.position - transform.position).normalized;
            dir.y = 0.0f;  // prevent vertical movement
            if (dir != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(dir);
            }
        }
        else if (moveDirection.sqrMagnitude > Data.MovementThreshold)
        {
            targetRotation = Quaternion.LookRotation(moveDirection);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * Data.TurnSpeed);
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    #endregion

    #region Animation Triggers
    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentState.AnimationTriggerEvent(triggerType);

    }

    public enum AnimationTriggerType  //TODO: Update list once animati0ons are ready
    {
        EndOfAttack,
        HitImpact
    }

    public void FadeLayerWeight(int layerIndex, float targetWeight, float duration)
    {
        if (_weightCoroutine != null)
        {
            StopCoroutine(_weightCoroutine);
        }

        _weightCoroutine = StartCoroutine(AnimateLayerWeight(layerIndex, targetWeight, duration));
    }

    private IEnumerator AnimateLayerWeight(int layerIndex, float targetWeight, float duration)
    {
        if (duration <= 0.0f)
        {
            anim.SetLayerWeight(layerIndex, targetWeight);
            yield break; // Exit the coroutine early
        }

        float startWeight = anim.GetLayerWeight(layerIndex);
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newWeight = Mathf.Lerp(startWeight, targetWeight, Mathf.Clamp01(elapsed / duration));
            anim.SetLayerWeight(layerIndex, newWeight);
            yield return null;
        }

        anim.SetLayerWeight(layerIndex, targetWeight);
        _weightCoroutine = null;
    }

    public void SetLayerWeight(int layerIndex, float weight)
    {
        // For instant change
        anim.SetLayerWeight(layerIndex, weight);
    }

    public void UpdateVisualSpeed(float currentSpeed)
    {
        anim.SetFloat("Speed", currentSpeed);
    }

    public void PlayAttack()
    {
        anim.SetTrigger("Attack");
    }

    public void ResetAttackTrigger()
    {
        anim.ResetTrigger("Attack");
    }

    public void PlayBlock()
    {
        anim.SetTrigger("Block");
    }

    public void ResetBlockTrigger()
    {
        anim.ResetTrigger("Block");
    }

    public void PlayDeathVisuals()
    {
        anim.SetBool("IsDead", true);
        anim.SetTrigger("Die");
    }
    #endregion
}
