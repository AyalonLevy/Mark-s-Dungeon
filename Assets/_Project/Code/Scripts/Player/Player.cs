using UnityEngine;

public class Player : MonoBehaviour, IDamagable, IMoveable
{
    [SerializeField] private PlayerData playerData;

    public float MaxHealth => playerData.MaxHealth;
    public float CurrentHealth { get; set; }
    public Rigidbody RB { get; set; }
    public bool IsFacingRight { get; set; } = true;  // based on the default gameobject facing direction

    #region State Machine Variables

    public PlayerStateMachine StateMachine { get; set; }
    public PlayerIdleState IdleState { get; set; }
    public PlayerMoveState MoveState { get; set; }
    public PlayerAttackState AttackState { get; set; }

    #endregion

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();

        StateMachine = new PlayerStateMachine();
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    #region Health / Die Functions

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //TODO: Death logic - Animation + sounds + gameover etc...
        Destroy(gameObject);
    }

    #endregion

    #region Movement Functions

    public void Move(Vector3 velocity)
    {
        RB.linearVelocity = velocity;
        CheckForLeftOrRight(velocity); 
    }

    public void CheckForLeftOrRight(Vector3 velocity)
    {
        if (IsFacingRight && velocity.x < 0.0f)
        {
            Vector3 rotator = new(transform.rotation.x, 180.0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
        else if (!IsFacingRight && velocity.x > 0.0f)
        {
            Vector3 rotator = new(transform.rotation.x, 0.0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
    }

    #endregion

    #region Animation Triggers

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentPlayerState.AnimationTriggerEvent(triggerType);

    }

    public enum AnimationTriggerType
    {
        EnemyDamage,
        PlayFootstepSound
    }

    #endregion
}
