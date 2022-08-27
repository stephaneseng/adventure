using UnityEngine;

public class EnemyMoveState : EnemyState
{
    private static float StateDurationInSeconds = 1.0f;

    public override void OnEnterState(EnemyStateMachine enemyStateMachine)
    {
        base.OnEnterState(enemyStateMachine);

        enemyStateMachine.GetEnemyController().StartMove();
    }

    public override void Update(EnemyStateMachine enemyStateMachine)
    {
        if (Time.time - startTime > StateDurationInSeconds) {
            enemyStateMachine.GetEnemyController().Attack();

            enemyStateMachine.SwitchState(new EnemyIdleState());
        }
    }
}
