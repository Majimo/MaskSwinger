using Godot;

public abstract partial class Feature : Node
{
	protected Cooldown Cooldown { get; set; }
	
	protected Player Player => Owner as Player;

	public override void _Ready()
	{
		this.Cooldown = new Cooldown();
		
		AddChild(this.Cooldown);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!this.CanExecuteFeature())
		{
			return;
		}

		this.DoFeature(delta);
	}

	protected virtual bool CanExecuteFeature()
		=> this.Cooldown.CanExecute() && !this.Player.IsDashing && !this.Player.IsAttacking && !this.Player.IsShielding;
	
	protected abstract void DoFeature(double delta);
}
