using System.Linq;
using Godot;
using MaskSwinger.Player;

[GlobalClass]
public partial class PlayerBehavior : Resource
{
	[Export] public float AttackDuration { get; set; } = 1f;
	[Export] public float AttackCooldown { get; set; } = 2f;

	[Export] public float ShieldDuration { get; set; } = 1f;
	[Export] public float ShieldSpeedDivider { get; set; } = 2f;
	[Export] public float ShieldCooldown { get; set; } = 2f;
	
	[Export] public float DashSpeedFactor { get; set; } = 5f;
	[Export] public float DashDuration { get; set; } = 0.1f;
	[Export] public float DashCooldown { get; set; } = 2f;
	
	[Export] public float Speed = 40.0f;
	
	#region attack
	
	public virtual void Attack(Player player, Vector3 direction)
	{
		player.ExecuteAfter(AttackDuration, () =>
		{
			player.IsAttacking = false;
		});
		
		var t = Mathf.Atan2(direction.Z, direction.X);

		var dir = t switch
		{
			>= -Mathf.Pi / 4 and < Mathf.Pi / 4 => Direction.Right,
			>= Mathf.Pi / 4 and < 3 * Mathf.Pi / 4 => Direction.Down,
			>= 3 * Mathf.Pi / 4 or < -3 * Mathf.Pi / 4 => Direction.Left,
			_ => Direction.Up
		};

		var hitZone = player.GetNode<Area3D>($"%AttackZone{dir}");

		var hits = hitZone.GetOverlappingBodies().OfType<Player>();
		
		foreach (var hit in hits)
		{
			hit.Hit();
		}
	}
	
	#endregion
	
	#region dash
	
    public virtual void Dash(Player player)
    {
        var dashVelocity = player.Velocity * DashSpeedFactor;
        player.Velocity = dashVelocity;
			
        player.ExecuteAfter(DashDuration, () => player.IsDashing = false);
	}

    #endregion
    
    #region shield
    
	public virtual void Shield(Player player)
	{
		var initialSpeed = this.Speed;
		
		this.Speed /= ShieldSpeedDivider;

		player.ExecuteAfter(ShieldDuration, () =>
		{
			player.IsShielding = false;
			this.Speed = initialSpeed;
		});
	}

	#endregion
}
