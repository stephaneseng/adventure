public class PlayerDestroyState : PlayerState
{
    public override void OnEnter(PlayerController playerController)
    {
        playerController.Destroy();
    }

    public override void OnUpdate(PlayerController playerController)
    {
    }

    public override void OnExit(PlayerController playerController)
    {
    }
}
