public class EnemyStateMachine
{
    private EnemyState currentState;

    public EnemyController enemyController;

    public EnemyStateMachine(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Start(EnemyState enemyState)
    {
        currentState = enemyState;
        currentState.OnEnterState(this);
    }

    public void Update()
    {
        currentState.Update(this);
    }

    public void SwitchState(EnemyState enemyState)
    {
        currentState.OnExitState(this);
        currentState = enemyState;
        currentState.OnEnterState(this);
    }
}
