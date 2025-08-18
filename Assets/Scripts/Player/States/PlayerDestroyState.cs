public class PlayerDestroyState : PlayerState
{
    public PlayerDestroyState(PlayerController playerController) : base(playerController)
    {
    }

    public override void OnEnter()
    {
        playerController.Destroy();
    }
}
