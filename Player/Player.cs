using Godot;
using MaskSwinger.Player;

public partial class Player : Node3D
{
	[Export] public int PlayerId { get; set; } = 1;
	[Export] public int Health { get; set; } = 3;
	[Export] public float Speed = 25.0f;
	
	[Export] public bool IsDashing { get; set; } 
	[Export] public bool IsShielding { get; set; } 
	[Export] public bool IsAttacking { get; set; } 
	
	public PlayerBehavior Behavior = new PlayerBehavior();
	public Direction CurrentDirection;
	public CharacterBody3D Body3D => GetNode<CharacterBody3D>("PlayerBody");
	
	private Vector3 _velocity = Vector3.Zero;

	public override void _Process(double delta)
	{
		if (!IsDashing)
		{
			var direction = Vector3.Zero;
			
			if (Input.IsActionPressed($"player_{this.PlayerId}_Up"))
			{
				CurrentDirection = Direction.Up;
				direction.Z = -1.0f;
			}
			if (Input.IsActionPressed($"player_{this.PlayerId}_Down"))
			{
				CurrentDirection = Direction.Down;
				direction.Z = 1.0f;
			}
			if (Input.IsActionPressed($"player_{this.PlayerId}_Left"))
			{
				CurrentDirection = Direction.Left;
				direction.X = -1.0f;
			}
			if (Input.IsActionPressed($"player_{this.PlayerId}_right"))
			{
				CurrentDirection = Direction.Right;
				direction.X = 1.0f;
			}

			_velocity.X = direction.X * Speed;
			_velocity.Y = direction.Y * Speed;
			_velocity.Z = direction.Z * Speed;
		
			Body3D.Velocity = _velocity;
		}
		Body3D.MoveAndSlide();
	}
}
