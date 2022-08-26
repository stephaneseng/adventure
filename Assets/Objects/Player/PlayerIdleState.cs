public class PlayerIdleState : PlayerState
{
    public override void Update(PlayerStateMachine playerStateMachine)
    {
        if (playerStateMachine.playerController.ReadInputActionMoveVector().magnitude > 0
            && !playerStateMachine.playerController.IsMoveLocked()) {

            playerStateMachine.SwitchState(new PlayerMoveState());
        }
    }
}
