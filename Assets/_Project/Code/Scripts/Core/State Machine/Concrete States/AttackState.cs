using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackState : EntityState
{
    private bool _isAnimationFinished;

    public AttackState(Entity entity, EntityStateMachine stateMachine) : base(entity, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _isAnimationFinished = false;

        entity.Move(Vector3.zero, Vector3.zero);

        //TODO: Trigger the animation
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (_isAnimationFinished)
        {
            stateMachine.ChangeState(entity.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationTriggerEvent(Entity.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);

        if (triggerType == Entity.AnimationTriggerType.EndOfAttack)
        {
            _isAnimationFinished = true;
        }
    }
}
