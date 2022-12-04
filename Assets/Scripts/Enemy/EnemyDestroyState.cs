using UnityEngine;

public class EnemyDestroyState : EnemyState
{
    private static float DestroyAnimationDurationInSeconds = 0.15f;

    public override void OnEnterState(EnemyStateMachine enemyStateMachine)
    {
        base.OnEnterState(enemyStateMachine);

        enemyStateMachine.GetEnemyController().Destroy();
    }

    public override void Update(EnemyStateMachine enemyStateMachine)
    {
        GameObject.Destroy(enemyStateMachine.GetEnemyController().gameObject, DestroyAnimationDurationInSeconds);
    }
}
