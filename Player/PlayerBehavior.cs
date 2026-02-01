using System.Linq;
using Godot;
using MaskSwinger.Player;

[GlobalClass]
public partial class PlayerBehavior : Resource
{
	[Export] public float AttackDuration { get; set; } = 1f;
	[Export] public float AttackCooldown { get; set; } = 2f;

	[Export] public float ShieldDuration { get; set; } = 1f;
	[Export] public float ShieldSpeedDivider { get; set; } = 4f;
	[Export] public float ShieldCooldown { get; set; } = 4f;
	
	[Export] public float DashSpeedFactor { get; set; } = 5f;
	[Export] public float DashDuration { get; set; } = 0.2f;
	[Export] public float DashCooldown { get; set; } = 2f;

	[Export] public float Speed = 40.0f;
	
	[Export] public SpriteFrames AvatarFrames { get; set; } = GD.Load<SpriteFrames>("res://2d assets/animations/base.tres");
	[Export] public Texture2D MaskTexture { get; set; } = GD.Load<Texture2D>("res://icon.svg");
	[Export] public Color MaskColor { get; set; } = Colors.White;
	
	#region attack
	
	public virtual void Attack(Player player, Direction direction)
	{
		player.PlayAttackAnimation(direction);
		
		player.ExecuteAfter(AttackDuration, () =>
		{
			player.IsAttacking = false;
		});
		
		var hitZone = player.GetNode<Area3D>($"%AttackZone{direction}");

		var hits = hitZone.GetOverlappingBodies().OfType<Player>();
		
		foreach (var hit in hits)
		{
			hit.Hit(player);
		}
	}
	
	#endregion
	
	#region dash
	
	public virtual void Dash(Player player)
	{
		player.PlayDashAnimation();
		
		var dashVelocity = player.Velocity * DashSpeedFactor;
		player.Velocity = dashVelocity;
			
		player.ExecuteAfter(DashDuration, () => player.IsDashing = false);
	}

	#endregion
	
	#region shield
	
	public virtual void Shield(Player player)
	{
		player.PlayShieldAnimation();
		
		var initialSpeed = this.Speed;
		
		this.Speed /= ShieldSpeedDivider;

		player.ExecuteAfter(ShieldDuration, () =>
		{
			this.Speed = initialSpeed;
			player.IsShielding = false;
		});
	}

	#endregion
}
