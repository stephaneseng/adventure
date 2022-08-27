public class EnemyStateMachine
{
    private EnemyState currentState;

    private EnemyController enemyController;

    public EnemyStateMachine(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public EnemyController GetEnemyController()
    {
        return enemyController;
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
