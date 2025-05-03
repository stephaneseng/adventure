public abstract class EnemyState
{
    protected EnemyController enemyController;

    protected EnemyState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public abstract void OnEnter();

    public abstract void OnUpdate();

    public abstract void OnExit();

    public EnemyController EnemyController => enemyController;
}
