public class PlayerIdleState : PlayerState
{
    public override void OnEnter(PlayerController playerController)
    {
    }

    public override void OnUpdate(PlayerController playerController)
    {
        if (playerController.ReadInputActionMoveVector().magnitude > 0)
        {
            playerController.playerStateMachine.SwitchState(new PlayerMoveState());
        }
    }

    public override void OnExit(PlayerController playerController)
    {
    }
}
