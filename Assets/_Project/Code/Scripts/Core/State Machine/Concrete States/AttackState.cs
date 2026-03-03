using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackState : EntityState
{
    private bool _isAnimationFinished;
    private MeleeWeapon _currentWeapon;

    public AttackState(Entity entity, EntityStateMachine stateMachine) : base(entity, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        entity.FadeLayerWeight(1, 1.0f, 0.0f);

        entity.SetRigActive(false);

        _isAnimationFinished = false;
        _currentWeapon = entity.WeaponSocket.GetComponentInChildren<MeleeWeapon>();

        entity.ResetAttackTrigger();

        //TODO: Trigger the animation
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

        if (_currentWeapon == null)
        {
            return;
        }

        if (triggerType == Entity.AnimationTriggerType.HitImpact)
        {
            _currentWeapon.Initialize(entity.Data.AttackDamage, entity.Data.EnemyLayer);
            _currentWeapon.SetActiveState(true);
        }

        if (triggerType == Entity.AnimationTriggerType.EndOfAttack)
        {
            _currentWeapon.SetActiveState(false);
        }
    }
}
