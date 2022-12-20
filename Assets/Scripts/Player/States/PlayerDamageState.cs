public class PlayerDamageState : PlayerState
{
    public override void OnEnter(PlayerController playerController)
    {
        playerController.Damage();
    }

    public override void OnUpdate(PlayerController playerController)
    {
        playerController.playerStateMachine.SwitchState(new PlayerIdleState());
    }

    public override void OnExit(PlayerController playerController)
    {
    }
}
