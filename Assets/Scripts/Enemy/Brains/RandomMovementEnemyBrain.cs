using UnityEngine;

[CreateAssetMenu(fileName = "RandomMovementEnemyBrain", menuName = "ScriptableObjects/RandomMovementEnemyBrain")]
public class RandomMovementEnemyBrain : EnemyBrain
{
    [SerializeField] private float idleStateDurationInSeconds = 0.5f;
    [SerializeField] private float moveStateDurationInSeconds = 1.0f;

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
        state.EnemyController.Move(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized);
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

    /* EnemyFreezeState */

    public override void OnEnter(EnemyFreezeState state)
    {
        state.EnemyController.StopMove();
    }
}
