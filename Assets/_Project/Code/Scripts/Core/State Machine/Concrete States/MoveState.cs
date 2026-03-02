using UnityEngine;

public class MoveState : EntityState
{
    public MoveState(Entity entity, EntityStateMachine stateMachine) : base(entity, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (entity.IsAttacking())
        {
            stateMachine.ChangeState(entity.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Vector2 rawDirection = entity.GetMoveInput();
        bool isSprinting = entity.IsSprinting() && entity.CurrentStamina > 0.0f;

        float speed = isSprinting ? entity.Data.SprintSpeed : entity.Data.WalkSpeed;

        Vector3 moveDirection = new(rawDirection.x, 0.0f, rawDirection.y);

        entity.Move(moveDirection * speed, Vector3.zero);

        if (isSprinting && moveDirection.sqrMagnitude > entity.Data.MovementThreshold)
        {
            entity.CurrentStamina = Mathf.Max(0, entity.CurrentStamina - (entity.Data.StaminaDrainRate * Time.fixedDeltaTime));
        }

        if (entity.GetMoveInput().sqrMagnitude < entity.Data .MovementThreshold)
        {
            stateMachine.ChangeState(entity.IdleState);
        }
    }

    public override void AnimationTriggerEvent(Entity.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
