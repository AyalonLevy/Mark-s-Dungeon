using UnityEngine;

public class Enemy : Entity
{
    [Header("AI Target")]
    [SerializeField] protected Transform target;
    protected override void Awake()
    {
        base.Awake();

        IdleState = new IdleState(this, StateMachine);
        MoveState = new MoveState(this, StateMachine);
        AttackState = new AttackState(this, StateMachine);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();

        StateMachine.CurrentState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #region Health / Die Functions

    protected override void OnDeath()
    {
        //TODO: Trigger gameover
        Debug.Log("Alas I died! (enmy/companion in case you forgot....)");
    }

    #endregion

    #region Input Implementation

    public override Vector2 GetMoveInput()
    {
        if (target == null)
        {
            return Vector2.zero;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= Data.AggroRange && distance > Data.AttackRange)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            return new(direction.x, direction.z);
        }

        return Vector2.zero;
    }

    public override bool IsAttacking()
    {
        if (target == null)
        {
            return false;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        return distance <= Data.AttackRange;
    }

    public override bool IsSprinting()
    {
        //TODO for now, we'll see if enemies will have sprint speen while in aggro mode or something
        return false;
    }

    #endregion

    #region Helper Functions

    private void OnDrawGizmosSelected()
    {
        if (Data == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Data.AggroRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Data.AttackRange);
    }

    #endregion
}
