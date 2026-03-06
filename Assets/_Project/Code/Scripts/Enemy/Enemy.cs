using UnityEngine;

public class Enemy : Entity
{
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
        Debug.Log("Alas I died! (enmy/companion in case you forgot....)");
    }

    #endregion

    #region Input Implementation

    public override Vector2 GetMoveInput()
    {
        if (currentTarget == null || _isDead)
        {
            return Vector2.zero;
        }

        Vector3 dirToTarget = (currentTarget.position - transform.position);

        float distSqr = dirToTarget.sqrMagnitude;
        float aggroRangeSqr = Data.AggroRange * Data.AggroRange;
        float attackRangeSqr = Data.AttackRange * Data.AttackRange;

        if (distSqr <= aggroRangeSqr && distSqr > attackRangeSqr)
        {
            Vector3 direction = dirToTarget.normalized;
            return new(direction.x, direction.z);
        }

        return Vector2.zero;
    }

    public override bool IsAttacking()
    {
        if (currentTarget == null)
        {
            return false;
        }

        float distSqr = (currentTarget.position - transform.position).sqrMagnitude;
        float attackRangeSqr = Data.AttackRange * Data.AttackRange;

        return distSqr <= attackRangeSqr && CanAttack();
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

        // Visualizing the search "Pulse" range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Data.DetectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Data.AggroRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Data.AttackRange);
    }

    #endregion
}
