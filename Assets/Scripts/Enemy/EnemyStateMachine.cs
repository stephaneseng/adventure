using UnityEngine;

public class EnemyStateMachine
{
    private EnemyController enemyController;

    public float startTime;

    private EnemyState currentState;

    public EnemyStateMachine(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Initialize(EnemyState enemyState)
    {
        startTime = Time.time;
        currentState = enemyState;
        currentState.OnEnter(enemyController);
    }

    public void Update()
    {
        currentState.OnUpdate(enemyController);
    }

    public void SwitchState(EnemyState enemyState)
    {
        currentState.OnExit(enemyController);

        startTime = Time.time;
        currentState = enemyState;
        currentState.OnEnter(enemyController);
    }
}
