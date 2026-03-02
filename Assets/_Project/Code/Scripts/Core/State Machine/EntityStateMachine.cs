public class EntityStateMachine
{
    public EntityState CurrentState { get; set; }

    public void Initialize(EntityState startingState)
    {
        CurrentState = startingState;
        CurrentState.EnterState();
    }

    public void ChangeState(EntityState newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
