public class PlayerStateMachine
{
    private PlayerState currentState;

    private PlayerController playerController;

    public PlayerStateMachine(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }

    public void Start(PlayerState playerState)
    {
        currentState = playerState;
        currentState.OnEnterState(this);
    }

    public void Update()
    {
        currentState.Update(this);
    }

    public void SwitchState(PlayerState playerState)
    {
        currentState.OnExitState(this);
        currentState = playerState;
        currentState.OnEnterState(this);
    }
}
