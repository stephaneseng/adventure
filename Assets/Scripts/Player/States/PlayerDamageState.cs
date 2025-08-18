public class PlayerDamageState : PlayerState
{
    public PlayerDamageState(PlayerController playerController) : base(playerController)
    {
    }

    public override void OnEnter()
    {
        playerController.Damage();
    }

    public override void OnUpdate()
    {
        playerController.SwitchToIdleState();
    }
}
