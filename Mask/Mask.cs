using Godot;

public partial class Mask : Node3D
{
	private Area3D _area => GetNode<Area3D>("Area");
	
	[Export] public PlayerBehavior Behavior;

	public override void _Ready()
	{
		_area.BodyEntered += AreaOnBodyEntered;

		var tween = CreateTween();

		tween.TweenProperty(this, "position", this.Position with { Z = 3 }, 2);
		tween.TweenProperty(this, "position", this.Position with { Z = 1 }, 2);

		tween.SetLoops();
	}

	private void AreaOnBodyEntered(Node3D body)
	{
		if (body is not Player player)
		{
			return;
		}
		
		if (player.Behavior is not null)
		{
			// TODO : drop mask	
		}
		
		// TODO : add mask to player
		//player.Behavior = this.Behavior;
	}
}
