public class EnemyDamageState : EnemyState
{
    public override void OnEnterState(EnemyStateMachine enemyStateMachine)
    {
        base.OnEnterState(enemyStateMachine);

        enemyStateMachine.GetEnemyController().PlayAnimation("Damage");
    }

    public override void Update(EnemyStateMachine enemyStateMachine)
    {
        enemyStateMachine.SwitchState(new EnemyIdleState());
    }
}
