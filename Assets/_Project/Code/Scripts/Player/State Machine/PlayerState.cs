public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine playerStateMachine;
    protected InputReader inputReader;
    protected PlayerData playerData;

    public PlayerState(Player player, PlayerStateMachine playerStateMachine, InputReader inputReader, PlayerData playerData)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
        this.inputReader = inputReader;
        this.playerData = playerData;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent(Player.AnimationTriggerType triggerType) { }
}
