using UnityEngine;

[CreateAssetMenu(fileName = "OrthographicMovementEnemyBrain", menuName = "ScriptableObjects/OrthographicMovementEnemyBrain")]
public class OrthographicMovementEnemyBrain : EnemyBrain
{
    private static readonly Vector2[] NextDirectionChoices =
    {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left,
        Vector2.zero
    };

    [SerializeField] private float idleStateDurationInSeconds = 0.5f;
    [SerializeField] private float moveStateDurationInSeconds = 1.0f;
    [SerializeField] private float destroyStateDurationInSeconds = 0.15f;

    /* EnemyIdleState */

    public override void OnUpdate(EnemyIdleState state)
    {
        if (Time.time - state.EnemyController.StateStartTime() > idleStateDurationInSeconds)
            state.EnemyController.SwitchToMoveState();
    }

    /* EnemyMoveState */

    public override void OnEnter(EnemyMoveState state)
    {
        // Choose a random direction.
        Vector2 nextDirection = NextDirectionChoices[Random.Range(0, NextDirectionChoices.Length)];
        if (nextDirection != Vector2.zero)
            state.EnemyController.Move(nextDirection);
        else
            state.EnemyController.StopMove();
    }

    public override void OnUpdate(EnemyMoveState state)
    {
        if (Time.time - state.EnemyController.StateStartTime() > moveStateDurationInSeconds)
            state.EnemyController.SwitchToAttackState();
    }

    public override void OnExit(EnemyMoveState state)
    {
        state.EnemyController.StopMove();
    }

    /* EnemyAttackState */

    public override void OnEnter(EnemyAttackState state)
    {
        state.EnemyController.Attack();
    }

    public override void OnUpdate(EnemyAttackState state)
    {
        state.EnemyController.SwitchToIdleState();
    }

    /* EnemyDamageState */

    public override void OnEnter(EnemyDamageState state)
    {
        state.EnemyController.Damage();
    }

    public override void OnUpdate(EnemyDamageState state)
    {
        state.EnemyController.SwitchToIdleState();
    }

    /* EnemyDestroyState */

    public override void OnEnter(EnemyDestroyState state)
    {
        state.EnemyController.Destroy();
    }

    public override void OnUpdate(EnemyDestroyState state)
    {
        Destroy(state.EnemyController.gameObject, destroyStateDurationInSeconds);
    }

    /* EnemyFreezeState */

    public override void OnEnter(EnemyFreezeState state)
    {
        state.EnemyController.StopMove();
    }
}
