using UnityEngine;

public abstract class EnemyBrain : ScriptableObject
{
    /* EnemyIdleState */

    public virtual void OnEnter(EnemyIdleState state)
    {
    }

    public virtual void OnUpdate(EnemyIdleState state)
    {
    }

    public virtual void OnExit(EnemyIdleState state)
    {
    }

    /* EnemyMoveState */

    public virtual void OnEnter(EnemyMoveState state)
    {
    }

    public virtual void OnUpdate(EnemyMoveState state)
    {
    }

    public virtual void OnExit(EnemyMoveState state)
    {
    }

    /* EnemyAttackState */

    public virtual void OnEnter(EnemyAttackState state)
    {
    }

    public virtual void OnUpdate(EnemyAttackState state)
    {
    }

    public virtual void OnExit(EnemyAttackState state)
    {
    }

    /* EnemyDamageState */

    public virtual void OnEnter(EnemyDamageState state)
    {
    }

    public virtual void OnUpdate(EnemyDamageState state)
    {
    }

    public virtual void OnExit(EnemyDamageState state)
    {
    }

    /* EnemyDestroyState */

    public virtual void OnEnter(EnemyDestroyState state)
    {
    }

    public virtual void OnUpdate(EnemyDestroyState state)
    {
    }

    public virtual void OnExit(EnemyDestroyState state)
    {
    }

    /* EnemyFreezeState */

    public virtual void OnEnter(EnemyFreezeState state)
    {
    }

    public virtual void OnUpdate(EnemyFreezeState state)
    {
    }

    public virtual void OnExit(EnemyFreezeState state)
    {
    }
}
