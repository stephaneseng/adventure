public abstract class EnemyState
{
    protected EnemyController enemyController;

    protected EnemyState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public EnemyController EnemyController => enemyController;

    public abstract void OnEnter();

    public abstract void OnUpdate();

    public abstract void OnExit();
}
