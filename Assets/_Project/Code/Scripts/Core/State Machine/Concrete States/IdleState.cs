using UnityEngine;


public class IdleState : EntityState
{
    public IdleState(Entity entity, EntityStateMachine stateMachine) : base(entity, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        entity.Move(Vector3.zero, Vector3.zero);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (entity.GetMoveInput().sqrMagnitude > entity.Data.MovementThreshold)
        {
            stateMachine.ChangeState(entity.MoveState);
        }

        if (entity.IsAttacking())
        {
            stateMachine.ChangeState(entity.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationTriggerEvent(Entity.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
