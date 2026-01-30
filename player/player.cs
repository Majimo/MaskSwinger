using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class player : CharacterBody2D
{
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
		
		Velocity = _velocity;
		MoveAndSlide();
	}
}
