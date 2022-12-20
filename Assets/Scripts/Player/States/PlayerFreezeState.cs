public class PlayerFreezeState : PlayerState
{
    public override void OnEnter(PlayerController playerController)
    {
        playerController.StopMove();
    }

    public override void OnUpdate(PlayerController playerController)
    {
    }

    public override void OnExit(PlayerController playerController)
    {
    }
}
