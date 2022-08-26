using UnityEngine;

public abstract class PlayerState
{
    protected float startTime;

    public virtual void OnEnterState(PlayerStateMachine playerStateMachine)
    {
        startTime = Time.time;
    }

    public virtual void Update(PlayerStateMachine playerStateMachine)
    {
    }

    public virtual void OnExitState(PlayerStateMachine playerStateMachine)
    {
    }
}
