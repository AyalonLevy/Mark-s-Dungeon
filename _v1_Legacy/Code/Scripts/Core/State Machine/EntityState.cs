using UnityEngine;

public abstract class EntityState
{
    protected Entity entity;
    protected EntityStateMachine stateMachine;

    public EntityState(Entity entity, EntityStateMachine stateMachine)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent(Entity.AnimationTriggerType triggerType) { }
}
