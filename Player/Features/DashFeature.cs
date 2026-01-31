using Godot;
using MaskSwinger.Player;

[GlobalClass]
public partial class DashFeature : Feature
{
	public override void _Process(double delta)
	{
		if (
			!this.Player.IsDashing &&
			this.Cooldown.CanExecute() && 
			Input.IsActionPressed("player_" + this.Player.PlayerId + "_dash"))
		{
			this.Player.Behavior.Dash(this.Player, Player.CurrentDirection);
			this.Player.IsDashing = true;
		}

		if (this.Player.IsDashing)
		{
			this.Cooldown.Restart();
			
			Timer timer = new Timer();
			timer.WaitTime = this.Player.Behavior.DashDuration;
			timer.OneShot = true;
			timer.Timeout += () =>
			{
				this.Player.IsDashing = false;
				timer.QueueFree();
			};
			this.AddChild(timer);
			timer.Start();
			
			return;
		}
		
		if (!this.Cooldown.CanExecute() || this.Player.IsShielding || this.Player.IsAttacking)
		{
			return;
		}
	}
}
