using Godot;
using MaskSwinger.Player;

[GlobalClass]
public partial class DashFeature : Feature
{
	public override void _PhysicsProcess(double delta)
	{
		if (!this.Cooldown.CanExecute() || this.Player.IsDashing || this.Player.IsAttacking || this.Player.IsShielding)
		{
			return;
		}
		
		if (Input.IsActionPressed("player_" + this.Player.PlayerId + "_dash"))
		{
			this.Player.Behavior.Dash(this.Player, Player.CurrentDirection);
			this.Player.IsDashing = true;
			this.Cooldown.Restart();
		}
	}
}
