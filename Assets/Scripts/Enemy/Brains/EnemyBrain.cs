using UnityEngine;

public abstract class EnemyBrain : ScriptableObject
{

    /* EnemyIdleState */

    public virtual void OnEnter(EnemyIdleState state, EnemyController enemyController)
    {
    }

    public virtual void OnUpdate(EnemyIdleState state, EnemyController enemyController)
    {
    }

    public virtual void OnExit(EnemyIdleState state, EnemyController enemyController)
    {
    }

    /* EnemyMoveState */

    public virtual void OnEnter(EnemyMoveState state, EnemyController enemyController)
    {
    }

    public virtual void OnUpdate(EnemyMoveState state, EnemyController enemyController)
    {
    }

    public virtual void OnExit(EnemyMoveState state, EnemyController enemyController)
    {
    }

    /* EnemyAttackState */

    public virtual void OnEnter(EnemyAttackState state, EnemyController enemyController)
    {
    }

    public virtual void OnUpdate(EnemyAttackState state, EnemyController enemyController)
    {
    }

    public virtual void OnExit(EnemyAttackState state, EnemyController enemyController)
    {
    }

    /* EnemyDamageState */

    public virtual void OnEnter(EnemyDamageState state, EnemyController enemyController)
    {
    }

    public virtual void OnUpdate(EnemyDamageState state, EnemyController enemyController)
    {
    }

    public virtual void OnExit(EnemyDamageState state, EnemyController enemyController)
    {
    }

    /* EnemyDestroyState */

    public virtual void OnEnter(EnemyDestroyState state, EnemyController enemyController)
    {
    }

    public virtual void OnUpdate(EnemyDestroyState state, EnemyController enemyController)
    {
    }

    public virtual void OnExit(EnemyDestroyState state, EnemyController enemyController)
    {
    }
}
