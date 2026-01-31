using Godot;

[GlobalClass]
public partial class ShieldFeature : Feature
{
	public override void _Process(double delta)
	{
		if (!this.Cooldown.CanExecute() || this.Player.IsDashing || this.Player.IsAttacking || this.Player.IsShielding)
		{
			return;
		}
		
		if (Input.IsActionPressed("player_" + this.Player.PlayerId + "_shield"))
		{
			this.Player.Behavior.Shield(this.Player, Player.CurrentDirection);
			this.Player.IsShielding = true;
			this.Cooldown.Restart();
		}
	}
}
