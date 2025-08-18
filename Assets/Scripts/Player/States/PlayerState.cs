public abstract class PlayerState
{
    protected PlayerController playerController;

    protected PlayerState(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnExit()
    {
    }
}
