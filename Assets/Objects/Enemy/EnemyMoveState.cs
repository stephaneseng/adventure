using UnityEngine;

public class EnemyMoveState : EnemyState
{
    private static float StateDurationInSeconds = 1.0f;

    public override void OnEnterState(EnemyStateMachine enemyStateMachine)
    {
        base.OnEnterState(enemyStateMachine);

        enemyStateMachine.enemyController.StartMove();
    }

    public override void Update(EnemyStateMachine enemyStateMachine)
    {
        if (Time.time - startTime > StateDurationInSeconds) {
            enemyStateMachine.enemyController.Attack();

            enemyStateMachine.SwitchState(new EnemyIdleState());
        }
    }
}
