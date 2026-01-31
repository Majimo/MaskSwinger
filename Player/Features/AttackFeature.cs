using Godot;
using MaskSwinger.Player;

[GlobalClass]
public partial class AttackFeature : Feature
{
	public override void _Process(double delta)
	{
		if (this.Cooldown.CanExecute() && Input.IsActionPressed("player_" + this.Player.PlayerId + "_attack_up"))
		{
			this.Player.Behavior.Attack(this.Player, Direction.Up);
			this.Cooldown.Restart();
		}
		if (this.Cooldown.CanExecute() && Input.IsActionPressed("player_" + this.Player.PlayerId + "_attack_down"))
		{
			this.Player.Behavior.Attack(this.Player, Direction.Down);
			this.Cooldown.Restart();
		}
		if (this.Cooldown.CanExecute() && Input.IsActionPressed("player_" + this.Player.PlayerId + "_attack_left"))
		{
			this.Player.Behavior.Attack(this.Player, Direction.Left);
			this.Cooldown.Restart();
		}
		if (this.Cooldown.CanExecute() && Input.IsActionPressed("player_" + this.Player.PlayerId + "_attack_right"))
		{
			this.Player.Behavior.Attack(this.Player, Direction.Right);
			this.Cooldown.Restart();
		}
	}
}
