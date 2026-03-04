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
        if (CurrentState == newState)
        {
            return;
        }

        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
