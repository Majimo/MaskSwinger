using Godot;

public partial class Mask : Node3D
{
	private Area3D _area => GetNode<Area3D>("Area");
	
	[Export] public PlayerBehavior Behavior;
	
	public MaskSpawner Spawner { get; set; }

	public override void _Ready()
	{
		_area.BodyEntered += AreaOnBodyEntered;

		var tween = CreateTween();

		tween.TweenProperty(this, "position", this.Position with { Z = this.Position.Z + 3 }, 2);
		tween.TweenProperty(this, "position", this.Position with { Z = this.Position.Z }, 2);

		tween.SetLoops();

		var sprite = GetNode<Sprite3D>("Sprite");
		
		GD.Print($"Setting mask texture and color ({this.Behavior.MaskColor})");
		
		sprite.Texture = this.Behavior.MaskTexture;
		sprite.Modulate = this.Behavior.MaskColor;
	}

	private void AreaOnBodyEntered(Node3D body)
	{
		if (body is not Player player)
		{
			return;
		}

		this.Spawner.ReleaseMask();
		
		this.QueueFree();
		
		// if (player.Behavior is not null)
		// {
		// 	// TODO : drop mask	
		// }
		//
		// // TODO : add mask to player
		// //player.Behavior = this.Behavior;
	}
}
