using UnityEngine;

public abstract class EnemyState
{
    protected float startTime;

    public virtual void OnEnterState(EnemyStateMachine enemyStateMachine)
    {
        startTime = Time.time;
    }

    public virtual void Update(EnemyStateMachine enemyStateMachine)
    {
    }

    public virtual void OnExitState(EnemyStateMachine enemyStateMachine)
    {
    }
}
