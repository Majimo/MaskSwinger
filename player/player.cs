using Godot;
using Vector2 = Godot.Vector2;

public partial class player : Node2D
{
	[Export] public int PlayerId { get; set; } = 1;
	[Export] public int Health { get; set; } = 3;
	
	private CharacterBody2D _body2D => GetNode<CharacterBody2D>("PlayerBody2D");
	private float _speed = 400;
	private Vector2 _velocity = Vector2.Zero;

	public override void _Process(double delta)
	{
		var direction = Vector2.Zero;
		
		if (Input.IsActionPressed("ui_up"))
		{
			direction.Y = -1.0f;
		}
		if (Input.IsActionPressed("ui_down"))
		{
			direction.Y = 1.0f;
		}
		if (Input.IsActionPressed("ui_left"))
		{
			direction.X = -1.0f;
		}
		if (Input.IsActionPressed("ui_right"))
		{
			direction.X = 1.0f;
		}

		_velocity.X = direction.X * _speed;
		_velocity.Y = direction.Y * _speed;
		
		_body2D.Velocity = _velocity;
		_body2D.MoveAndSlide();
	}
}
