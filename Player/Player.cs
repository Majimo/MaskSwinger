using Godot;
using MaskSwinger.Player;

public partial class Player : CharacterBody3D
{
	[Export] public int PlayerId { get; set; } = 1;
	[Export] public int Health { get; set; } = 3;
	
	[Export] public float Speed = 25.0f;
	
	[Export] public bool IsDashing { get; set; } 
	[Export] public bool IsShielding { get; set; } 
	[Export] public bool IsAttacking { get; set; } 
	
	public PlayerBehavior Behavior = new PlayerBehavior();
	
	public Direction CurrentDirection;

	public override void _PhysicsProcess(double delta)
	{
		if (!IsDashing)
		{
			var direction = Vector3.Zero;
			
			if (Input.IsActionPressed($"player_{this.PlayerId}_up"))
			{
				CurrentDirection = Direction.Up;
				direction.Z = -1.0f;
			}
			if (Input.IsActionPressed($"player_{this.PlayerId}_down"))
			{
				CurrentDirection = Direction.Down;
				direction.Z = 1.0f;
			}
			if (Input.IsActionPressed($"player_{this.PlayerId}_left"))
			{
				CurrentDirection = Direction.Left;
				direction.X = -1.0f;
			}
			if (Input.IsActionPressed($"player_{this.PlayerId}_right"))
			{
				CurrentDirection = Direction.Right;
				direction.X = 1.0f;
			}

			if (direction != Vector3.Zero)
			{
				direction = direction.Normalized();
			}
			
			this.Velocity = direction * Speed;
		}
		
		this.MoveAndSlide();
	}
}
