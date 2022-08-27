using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public override void Update(PlayerStateMachine playerStateMachine)
    {
        Vector2 inputActionMoveVector = playerStateMachine.GetPlayerController().ReadInputActionMoveVector();

        if (inputActionMoveVector.magnitude > 0
            && !playerStateMachine.GetPlayerController().IsMoveLocked()) {

            playerStateMachine.GetPlayerController().Move(inputActionMoveVector);
        } else {
            playerStateMachine.GetPlayerController().StopMove();

            playerStateMachine.SwitchState(new PlayerIdleState());
        }
    }
}
