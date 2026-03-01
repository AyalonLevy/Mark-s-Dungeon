using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine, InputReader inputReader, PlayerData playerData) : base(player, playerStateMachine, inputReader, playerData)
    {
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Entered Idle State");

        player.Move(Vector3.zero);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (inputReader.movement.sqrMagnitude > playerData.MovementThreshold)
        {
            playerStateMachine.ChangeState(player.MoveState);
        }

        if (inputReader.IsAttacking)
        {
            playerStateMachine.ChangeState(player.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
