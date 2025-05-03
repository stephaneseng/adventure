public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController playerController) : base(playerController)
    {
    }

    public override void OnUpdate()
    {
        if (playerController.ReadInputActionMoveVector().magnitude > 0) playerController.SwitchToMoveState();
    }
}
