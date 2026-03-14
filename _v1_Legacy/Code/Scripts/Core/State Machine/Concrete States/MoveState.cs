using UnityEngine;

public class MoveState : EntityState
{
    public MoveState(Entity entity, EntityStateMachine stateMachine) : base(entity, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        entity.SetRigActive(false);
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
            stateMachine.ChangeState(entity.IdleState);
            return;
        }

        if (entity.IsAttacking())
        {
            stateMachine.ChangeState(entity.AttackState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Vector2 input = entity.GetMoveInput();

        bool isCalculating = false;

        if (entity.TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var navAgent))
        {
            isCalculating = navAgent.pathPending;
        }

        if (input.sqrMagnitude < entity.Data.MovementThreshold && !isCalculating)
        {
            stateMachine.ChangeState(entity.IdleState);
            return;
        }

        float speed = entity.GetMoveSpeed();
        Vector3 moveDirection = new(input.x, 0.0f, input.y);
        Vector3 velocity = moveDirection * speed;
        // To allow Y movement:
        //Vector3 velocity = new(input.x * speed, entity.RB.linearVelocity.y, input.y * speed);

        Debug.Log($"{entity.name}: Move velocity: {velocity}");
        entity.Move(velocity);
        entity.HandleOrientation(moveDirection);
        entity.UpdateVisualSpeed(velocity.magnitude);

        if (entity.IsSprinting() && velocity.sqrMagnitude > entity.Data.MovementThreshold)
        {
            entity.CurrentStamina = Mathf.Max(0, entity.CurrentStamina - (entity.Data.StaminaDrainRate * Time.fixedDeltaTime));
        }
    }

    public override void AnimationTriggerEvent(Entity.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
