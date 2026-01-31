using Godot;
using Vector2 = Godot.Vector2;

public partial class Player : Node3D
{
	[Export] public int PlayerId { get; set; } = 1;
	[Export] public int Health { get; set; } = 3;
	
	[Export] public bool IsDashing { get; set; } 
	[Export] public bool IsShielding { get; set; } 
	[Export] public bool IsAttacking { get; set; } 
	
	public PlayerBehavior Behavior = new PlayerBehavior();
	
	private CharacterBody3D _body3D => GetNode<CharacterBody3D>("PlayerBody");
	private Cooldown _attackCooldown => GetNode<Cooldown>("AttackCooldown");
	private float _speed = 50;
	private Vector3 _velocity = Vector3.Zero;

	public override void _Process(double delta)
	{
		var direction = Vector3.Zero;
		
		if (Input.IsActionPressed($"player_{this.PlayerId}_up"))
		{
			direction.Z = -1.0f;
		}
		if (Input.IsActionPressed($"player_{this.PlayerId}_down"))
		{
			direction.Z = 1.0f;
		}
		if (Input.IsActionPressed($"player_{this.PlayerId}_left"))
		{
			direction.X = -1.0f;
		}
		if (Input.IsActionPressed($"player_{this.PlayerId}_right"))
		{
			direction.X = 1.0f;
		}

		_velocity.X = direction.X * _speed;
		_velocity.Y = direction.Y * _speed;
		_velocity.Z = direction.Z * _speed;
		
		_body3D.Velocity = _velocity;
		_body3D.MoveAndSlide();
	}
}
