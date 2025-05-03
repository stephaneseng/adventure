public class PlayerFreezeState : PlayerState
{
    public PlayerFreezeState(PlayerController playerController) : base(playerController)
    {
    }

    public override void OnEnter()
    {
        playerController.StopMove();
    }
}
