using UnityEngine;

public class EnemyStateMachine
{
    private EnemyState currentState;
    private float startTime;

    public void Initialize(EnemyState enemyState)
    {
        currentState = enemyState;
        StartTime = Time.time;

        currentState.OnEnter();
    }

    public void Update()
    {
        currentState.OnUpdate();
    }

    public void SwitchState(EnemyState enemyState)
    {
        currentState.OnExit();

        currentState = enemyState;
        StartTime = Time.time;

        currentState.OnEnter();
    }

    public float StartTime { get; private set; }
}
