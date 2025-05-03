using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerController playerController) : base(playerController)
    {
    }

    public override void OnUpdate()
    {
        Vector2 inputActionMoveVector = playerController.ReadInputActionMoveVector();

        if (inputActionMoveVector.magnitude > 0)
            playerController.Move(inputActionMoveVector);
        else
            playerController.SwitchToIdleState();
    }

    public override void OnExit()
    {
        playerController.StopMove();
    }
}
