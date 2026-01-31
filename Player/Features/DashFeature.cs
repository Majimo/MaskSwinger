using Godot;
using MaskSwinger.Player;

[GlobalClass]
public partial class DashFeature : Feature
{
	public override void _Process(double delta)
	{
		if (this.Cooldown.CanExecute() && Player.IsDashing )
		{
			// check if dashing should be set to false
		}
		
		if (!this.Cooldown.CanExecute() || Player.IsShielding || Player.IsAttacking)
		{
			return;
		}
	}
}
