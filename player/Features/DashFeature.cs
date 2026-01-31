using Godot;
using MaskSwinger.Player;

[GlobalClass]
public partial class DashFeature : Feature
{
	public override void _Process(double delta)
	{
		if (Player.IsDashing)
		{
			// check if dashing should be set to false
		}
		
		if (!Cooldown.CanExecute() || Player.IsShielding || Player.IsAttacking)
		{
			return;
		}
		
		// if (Input.IsActionJustPressed("dash"))
		// {
		// 	// Get Direction
		// 	Player.Behavior.Dash(Player, Direction.Down);
		// 	Cooldown.Restart();
		// }
	}
}
