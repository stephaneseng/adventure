using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public override void OnEnter(PlayerController playerController)
    {
    }

    public override void OnUpdate(PlayerController playerController)
    {
        Vector2 inputActionMoveVector = playerController.ReadInputActionMoveVector();

        if (inputActionMoveVector.magnitude > 0)
        {
            playerController.Move(inputActionMoveVector);
        }
        else
        {
            playerController.playerStateMachine.SwitchState(new PlayerIdleState());
        }
    }

    public override void OnExit(PlayerController playerController)
    {
        playerController.StopMove();
    }
}
