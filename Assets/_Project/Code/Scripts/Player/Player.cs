using UnityEngine;

public class Player : Entity
{
    [Header("Dependencies")]
    [SerializeField] private InputReader inputReader;

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

    private void Update()
    {
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
        Debug.Log("I died! (player in case you forgot....)");
    }

    #endregion

    #region Input Implementation

    public override Vector2 GetMoveInput() => inputReader.movement;

    public override bool IsAttacking() => inputReader.IsAttacking;

    public override bool IsSprinting() => inputReader.IsSprinting;

    #endregion
}
