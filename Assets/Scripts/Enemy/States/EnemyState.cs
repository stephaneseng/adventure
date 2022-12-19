public abstract class EnemyState
{
    public abstract void OnEnter(EnemyController enemyController);

    public abstract void OnUpdate(EnemyController enemyController);

    public abstract void OnExit(EnemyController enemyController);
}
