using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public override void Update(PlayerStateMachine playerStateMachine)
    {
        Vector2 inputActionMoveVector = playerStateMachine.playerController.ReadInputActionMoveVector();

        if (inputActionMoveVector.magnitude > 0
            && !playerStateMachine.playerController.IsMoveLocked()) {

            playerStateMachine.playerController.Move(inputActionMoveVector);
        } else {
            playerStateMachine.playerController.StopMove();

            playerStateMachine.SwitchState(new PlayerIdleState());
        }
    }
}
