using UnityEngine;

[CreateAssetMenu(fileName = "OrthographicMovementEnemyBrain", menuName = "ScriptableObjects/OrthographicMovementEnemyBrain")]
public class OrthographicMovementEnemyBrain : EnemyBrain
{
    private static Vector2[] NextDirectionChoices = new Vector2[] {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left,
        Vector2.zero
    };

    public float idleStateDurationInSeconds = 0.5f;

    public float moveStateDurationInSeconds = 1.0f;

    public float destroyStateDurationInSeconds = 0.15f;

    /* EnemyIdleState */

    public override void OnUpdate(EnemyIdleState state, EnemyController enemyController)
    {
        if (Time.time - enemyController.enemyStateMachine.startTime > idleStateDurationInSeconds)
        {
            enemyController.enemyStateMachine.SwitchState(new EnemyMoveState());
        }
    }

    /* EnemyMoveState */

    public override void OnEnter(EnemyMoveState state, EnemyController enemyController)
    {
        // Choose a random target position.
        Vector2 nextDirection = NextDirectionChoices[Random.Range(0, NextDirectionChoices.Length)];
        if (nextDirection != Vector2.zero)
        {
            enemyController.direction = nextDirection;
            enemyController.StartMove();
        }
        else
        {
            enemyController.StopMove();
        }
    }

    public override void OnUpdate(EnemyMoveState state, EnemyController enemyController)
    {
        if (Time.time - enemyController.enemyStateMachine.startTime > moveStateDurationInSeconds)
        {
            enemyController.enemyStateMachine.SwitchState(new EnemyAttackState());
        }
    }

    public override void OnExit(EnemyMoveState state, EnemyController enemyController)
    {
        enemyController.StopMove();
    }

    /* EnemyAttackState */

    public override void OnEnter(EnemyAttackState state, EnemyController enemyController)
    {
        enemyController.Attack();
    }

    public override void OnUpdate(EnemyAttackState state, EnemyController enemyController)
    {
        enemyController.enemyStateMachine.SwitchState(new EnemyIdleState());
    }

    /* EnemyDamageState */

    public override void OnEnter(EnemyDamageState state, EnemyController enemyController)
    {
        enemyController.Damage();
    }

    public override void OnUpdate(EnemyDamageState state, EnemyController enemyController)
    {
        enemyController.enemyStateMachine.SwitchState(new EnemyIdleState());
    }

    /* EnemyDestroyState */

    public override void OnEnter(EnemyDestroyState state, EnemyController enemyController)
    {
        enemyController.Destroy();
    }

    public override void OnUpdate(EnemyDestroyState state, EnemyController enemyController)
    {
        GameObject.Destroy(enemyController.gameObject, destroyStateDurationInSeconds);
    }
}
