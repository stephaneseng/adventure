using UnityEngine;

public class PlayerStateMachine
{
    public PlayerController playerController;

    public float startTime;

    private PlayerState currentState;

    public PlayerStateMachine(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Initialize(PlayerState playerState)
    {
        startTime = Time.time;
        currentState = playerState;
        currentState.OnEnter(playerController);
    }

    public void Update()
    {
        currentState.OnUpdate(playerController);
    }

    public void SwitchState(PlayerState playerState)
    {
        currentState.OnExit(playerController);

        startTime = Time.time;
        currentState = playerState;
        currentState.OnEnter(playerController);
    }
}
