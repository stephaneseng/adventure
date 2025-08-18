public class PlayerStateMachine
{
    private PlayerState currentState;

    public void Initialize(PlayerState playerState)
    {
        currentState = playerState;

        currentState.OnEnter();
    }

    public void Update()
    {
        currentState.OnUpdate();
    }

    public void SwitchState(PlayerState playerState)
    {
        currentState.OnExit();

        currentState = playerState;

        currentState.OnEnter();
    }
}
