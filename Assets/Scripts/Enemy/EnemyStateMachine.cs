using UnityEngine;

public class EnemyStateMachine
{
    private EnemyState currentState;
    private float startTime;

    public float StartTime => startTime;

    public void Initialize(EnemyState enemyState)
    {
        currentState = enemyState;
        startTime = Time.time;

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
        startTime = Time.time;

        currentState.OnEnter();
    }
}
