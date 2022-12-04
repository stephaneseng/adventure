public class PlayerIdleState : PlayerState
{
    public override void Update(PlayerStateMachine playerStateMachine)
    {
        if (playerStateMachine.GetPlayerController().ReadInputActionMoveVector().magnitude > 0
            && !playerStateMachine.GetPlayerController().IsMoveLocked())
        {

            playerStateMachine.SwitchState(new PlayerMoveState());
        }
    }
}
