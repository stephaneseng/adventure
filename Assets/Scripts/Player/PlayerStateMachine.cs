using UnityEngine;

public class PlayerStateMachine
{
    private PlayerState currentState;
    private float startTime;

    public void Initialize(PlayerState playerState)
    {
        currentState = playerState;
        startTime = Time.time;

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
        startTime = Time.time;

        currentState.OnEnter();
    }

    public float StartTime => startTime;
}
