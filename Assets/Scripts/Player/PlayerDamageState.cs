public class PlayerDamageState : PlayerState
{
    public override void OnEnterState(PlayerStateMachine playerStateMachine)
    {
        base.OnEnterState(playerStateMachine);

        playerStateMachine.GetPlayerController().Damage();
    }

    public override void Update(PlayerStateMachine playerStateMachine)
    {
        playerStateMachine.SwitchState(new PlayerIdleState());
    }
}
