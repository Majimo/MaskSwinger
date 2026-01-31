using Godot;
using MaskSwinger.Player;

[GlobalClass]
public partial class DashFeature : Feature
{
	protected override void DoFeature(double delta)
	{
		if (!Input.IsActionPressed($"player_{this.Player.PlayerId}_dash"))
		{
			return;
		}
		
		this.Player.Behavior.Dash(this.Player);
		this.Player.IsDashing = true;
		this.Cooldown.Restart(this.Player.Behavior.DashCooldown);
	}
}
