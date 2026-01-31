using Godot;

[GlobalClass]
public partial class ShieldFeature : Feature
{
	protected override void DoFeature(double delta)
	{
		if (!Input.IsActionPressed($"player_{this.Player.PlayerId}_shield"))
		{
			return;
		}
		
		this.Player.Behavior.Shield(this.Player);
		this.Player.IsShielding = true;
		this.Cooldown.Restart(this.Player.Behavior.ShieldCooldown);
	}
}
