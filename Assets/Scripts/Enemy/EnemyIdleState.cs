using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private static float StateDurationInSeconds = 1.0f;

    public override void OnEnterState(EnemyStateMachine enemyStateMachine)
    {
        base.OnEnterState(enemyStateMachine);

        enemyStateMachine.GetEnemyController().StopMove();
    }

    public override void Update(EnemyStateMachine enemyStateMachine)
    {
        if (Time.time - startTime > StateDurationInSeconds) {
            enemyStateMachine.SwitchState(new EnemyMoveState());
        }
    }
}
