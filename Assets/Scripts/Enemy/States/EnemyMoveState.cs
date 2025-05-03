public class EnemyMoveState : EnemyState
{
    public EnemyMoveState(EnemyController enemyController) : base(enemyController)
    {
    }

    public override void OnEnter()
    {
        enemyController.EnemyBrain.OnEnter(this);
    }

    public override void OnUpdate()
    {
        enemyController.EnemyBrain.OnUpdate(this);
    }

    public override void OnExit()
    {
        enemyController.EnemyBrain.OnExit(this);
    }
}
