using UnityEngine;

public abstract class Entity : MonoBehaviour, IDamagable, IMoveable
{
    public EntityData Data;

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

        StateMachine = new EntityStateMachine();

        MaxHealth = Data.MaxHealth;
        _currentHealth = MaxHealth;
        CurrentMana = Data.MaxMana;
        CurrentStamina = Data.MaxStamina;
    }
    public abstract Vector2 GetMoveInput();
    public abstract bool IsAttacking();
    public abstract bool IsSprinting();

    #region Health / Die Functions

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0.0f)
        {
            Die();

            Destroy(gameObject);
        }
    }
    public void Die()
    {
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
    }

    #endregion
}
