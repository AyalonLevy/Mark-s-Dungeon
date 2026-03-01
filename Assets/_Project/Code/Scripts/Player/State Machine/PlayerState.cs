public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine playerStateMachine;
    protected InputReader inputReader;

    public PlayerState(Player player, PlayerStateMachine playerStateMachine, InputReader inputReader)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
        this.inputReader = inputReader;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent(Player.AnimationTriggerType triggerType) { }
}
