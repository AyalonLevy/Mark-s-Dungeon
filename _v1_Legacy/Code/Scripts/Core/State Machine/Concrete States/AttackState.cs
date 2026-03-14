using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackState : EntityState
{
    private bool _isAnimationFinished;
    private Weapon _currentWeapon;

    public AttackState(Entity entity, EntityStateMachine stateMachine) : base(entity, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        entity.ResetAttackCooldown();

        entity.FadeLayerWeight(1, 1.0f, 0.0f);

        entity.SetRigActive(false);

        _isAnimationFinished = false;
        _currentWeapon = entity.GetCurrentWeapon();

        entity.ResetAttackTrigger();

        entity.PlayAttack();
    }

    public override void ExitState()
    {
        base.ExitState();

        entity.FadeLayerWeight(1, 0.0f, 0.15f);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (_isAnimationFinished || entity.IsDead())
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

        if (_currentWeapon == null)
        {
            return;
        }

        if (triggerType == Entity.AnimationTriggerType.HitImpact)
        {
            _currentWeapon.SetActiveState(true);

            if (_currentWeapon is RangedWeapon ranged && entity.CurrentTarget != null)
            {
                Vector3 targetCenter = entity.CurrentTarget.position + Vector3.up;
                ranged.PerformAttack(targetCenter);
            }
            else 
            {
                _currentWeapon.PerformAttack();
            }
        }

        if (triggerType == Entity.AnimationTriggerType.EndOfAttack)
        {
            _currentWeapon.SetActiveState(false);
        }
    }
}
