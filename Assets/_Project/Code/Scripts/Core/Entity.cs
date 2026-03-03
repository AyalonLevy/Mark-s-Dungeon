using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;

public abstract class Entity : MonoBehaviour, IDamagable, IMoveable
{
    public EntityData Data;
    protected Animator anim;

    private Coroutine _weighttCoroutine;

    [Header("Sockets")]
    public Transform WeaponSocket;
    public Transform ShieldSocket;

    public float MaxHealth { get; set; }
    private float _currentHealth;
    public float CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = Mathf.Clamp(value, 0.0f, MaxHealth); 
    }
    public float CurrentMana { get; set; }
    public float CurrentStamina { get; set; }

    public Rigidbody RB { get; set; }
    public bool IsFacingRight { get; set; } = true;  // based on the default gameobject facing direction

    private bool _enemiesNearby;

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

    public void SetRigActive(bool active)
    {
        if (_rigBuilder != null)
        {
            _rigBuilder.enabled = active;
        }
    }

    protected virtual void Update()
    {
        CheckForCombatStatus();
    }

    private void CheckForCombatStatus()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Data.DetectionRange, Data.EnemyLayer);
        bool currentlyEnemiesNearby = hitColliders.Length > 0;

        if (currentlyEnemiesNearby != _enemiesNearby)
        {
            _enemiesNearby = hitColliders.Length > 0;
            anim.SetBool("InCombat", _enemiesNearby);
        }
    }

    public abstract Vector2 GetMoveInput();
    public abstract bool IsAttacking();
    public abstract bool IsSprinting();

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

    #region Health / Die Functions

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0.0f)
        {
            Die();

            //Destroy(gameObject);
        }
    }
    public void Die()
    {
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
        }

        if (TryGetComponent<Collider>(out Collider collider))
        {
            collider.enabled = false;
        }

        OnDeath();
    }

    protected abstract void OnDeath();

    #endregion

    #region Movement Functions

    public void Move(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        RB.linearVelocity = linearVelocity;
        RB.angularVelocity = angularVelocity;
        HandleFlip(linearVelocity);
    }

    public void HandleFlip(Vector3 velocity)
    {
        if (Mathf.Abs(velocity.x) < Data.MovementThreshold)
        {
            return;
        }

        if (IsFacingRight && velocity.x < 0.0f)
        {
            SetRotation(180.0f);
            IsFacingRight = false;
        }
        else if (!IsFacingRight && velocity.x > 0.0f)
        {
            SetRotation(0.0f);
            IsFacingRight = true;
        }
    }

    public void SetRotation(float yAngle)
    {
        transform.rotation = Quaternion.Euler(0.0f, yAngle, 0.0f);
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
        if (_weighttCoroutine != null)
        {
            StopCoroutine(_weighttCoroutine);
        }

        _weighttCoroutine = StartCoroutine(AnimateLayerWeight(layerIndex, targetWeight, duration));
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
        _weighttCoroutine = null;
    }

    public void SetLayerWeight(int layerIndex, float weight)
    {
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

    public void PlayDeathVisuals()
    {
        anim.SetBool("InCombat", false);
        anim.SetBool("IsDead", true);
        anim.SetTrigger("Die");
    }

    #endregion
}
