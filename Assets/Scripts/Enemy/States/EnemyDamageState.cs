public class EnemyDamageState : EnemyState
{
    public override void OnEnter(EnemyController enemyController)
    {
        enemyController.enemyData.enemyBrain.OnEnter(this, enemyController);
    }

    public override void OnUpdate(EnemyController enemyController)
    {
        enemyController.enemyData.enemyBrain.OnUpdate(this, enemyController);
    }

    public override void OnExit(EnemyController enemyController)
    {
        enemyController.enemyData.enemyBrain.OnExit(this, enemyController);
    }
}
