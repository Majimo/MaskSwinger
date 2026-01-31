using Godot;
using System;

public partial class Mask : Node3D
{
	private Area3D _area => GetNode<Area3D>("Area");
	
	[Export] public PlayerBehavior Behavior;

	public override void _Ready()
	{
		_area.BodyEntered += AreaOnBodyEntered;
	}

	private void AreaOnBodyEntered(Node3D body)
	{
		if (Behavior is not null && body is CharacterBody3D playerBody)
		{
			var player = playerBody.GetParent<Player>();

			if (player.Behavior is not null)
			{
				// TODO : drop mask	
			}
			
			this.Behavior = this.Behavior;
		}
	}
}
