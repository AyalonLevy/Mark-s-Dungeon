using UnityEngine;


public class IdleState : EntityState
{
    public IdleState(Entity entity, EntityStateMachine stateMachine) : base(entity, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();


        entity.SetRigActive(false);
        entity.UpdateVisualSpeed(0f);
        entity.Move(Vector3.zero);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (entity.IsDead())
        {
            return;
        }

        if (entity.IsAttacking())
        {
            stateMachine.ChangeState(entity.AttackState);
            return;
        }

        if (entity.GetMoveInput().sqrMagnitude > entity.Data.MovementThreshold)
        {
            Debug.Log($"{entity.name}: Going from Idle to Move");
            stateMachine.ChangeState(entity.MoveState);
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
