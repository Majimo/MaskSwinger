using Godot;
using MaskSwinger.Player;

public partial class Player : CharacterBody3D
{
	private const float DeadZone = 0.2f;
	
	private AnimatedSprite3D _sprite;
	private Direction _lastDirection;
	private AudioStreamPlayer _swingAudioPlayer;
	private AudioStreamPlayer _hitAudioPlayer;
	private AudioStreamPlayer _dashAudioPlayer;

	[Export] public int PlayerId { get; set; }
	
	[Export] public bool IsDashing { get; set; } 
	[Export] public bool IsShielding { get; set; } 
	[Export] public bool IsAttacking { get; set; } 
	
	public PlayerBehavior Behavior { get; private set; }

	public override void _Ready()
	{
		_sprite = this.GetNode<AnimatedSprite3D>("Animation");
		_swingAudioPlayer = this.GetNode<AudioStreamPlayer>("SwingPlayer");
		
		this.ChangeBehavior(new PlayerBehavior());
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!IsDashing && !IsAttacking)
		{
			var velocity = Vector3.Zero;

			var xAxis = Input.GetJoyAxis(this.PlayerId, JoyAxis.LeftX);
			if (Mathf.Abs(xAxis) > DeadZone)
			{
				velocity.X = xAxis;
			} 
			
			var yAxis = Input.GetJoyAxis(this.PlayerId, JoyAxis.LeftY);
			if (Mathf.Abs(yAxis) > DeadZone)
			{
				velocity.Z = yAxis;
			}

			if (velocity != Vector3.Zero)
			{
				velocity = velocity.Normalized();
				
				_lastDirection = GetDirection(velocity);
				
				_sprite.Call("_play_walk", (int)_lastDirection);
			}
			else
			{
				_sprite.Call("_play_idle", (int)_lastDirection);
			}
			
			this.Velocity = velocity * this.Behavior.Speed;
		}

		if (!IsAttacking)
		{
			this.MoveAndSlide();
		}
	}

	public void PlayAttackAnimation(Direction direction)
	{
		_swingAudioPlayer.Play();
		_sprite.Call("_play_atk", (int)direction);
	}
	
	public void PlayDashAnimation()
	{
		_sprite.Call("_play_dash", (int)_lastDirection);
	}
	
	public void PlayShieldAnimation()
	{
		_sprite.Call("_play_shield", (int)_lastDirection);
	}
	
	public void Hit(Player hitBy)
	{
		if (IsShielding)
		{
			return;
		}
		
		GameManager.Instance.Killing(this, hitBy);
		
		GetTree().CurrentScene.ExecuteAfter(2, () =>
		{
			GD.Print("Respawn !!!");
			this.ProcessMode = ProcessModeEnum.Inherit;
			this.Visible = true;
		});
			
		this.ProcessMode = ProcessModeEnum.Disabled;
		this.Visible = false;
	}

	public void ChangeBehavior(PlayerBehavior behavior)
	{
		this.Behavior = behavior;
		
		_sprite.SpriteFrames = behavior.AvatarFrames;
		_sprite.Modulate = behavior.MaskColor;
	}

	private static Direction GetDirection(Vector3 velocity)
	{
		var t = Mathf.Atan2(velocity.Z, velocity.X);

		var direction = t switch
		{
			>= -Mathf.Pi / 4 and < Mathf.Pi / 4 => Direction.Right,
			>= Mathf.Pi / 4 and < 3 * Mathf.Pi / 4 => Direction.Down,
			>= 3 * Mathf.Pi / 4 or < -3 * Mathf.Pi / 4 => Direction.Left,
			_ => Direction.Up
		};

		return direction;
	}
}
