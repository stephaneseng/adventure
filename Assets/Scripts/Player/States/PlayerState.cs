public abstract class PlayerState
{
    public abstract void OnEnter(PlayerController playerController);

    public abstract void OnUpdate(PlayerController playerController);

    public abstract void OnExit(PlayerController playerController);
}
