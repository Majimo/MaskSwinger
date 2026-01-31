using Godot;
using MaskSwinger.Player;

[GlobalClass]
public partial class PlayerBehavior : Resource
{
    [Export] public int AttackDamage;
    [Export] public int AttackCooldown;
    [Export] public int ShieldDuration;
    [Export] public int ShieldCooldown;
    [Export] public int DashSpeedFactor = 3;
    [Export] public int DashCooldown = 2;
    [Export] public float DashDuration { get; set; } = 0.2f;
    
    public virtual void Attack(Player player, Direction direction)
    {
        GD.Print("Attacking " + player.PlayerId + " towards " + direction);
    }

    public virtual void Dash(Player player, Direction playerDirection)
    {
        Vector3 dashVelocity = player.Body3D.Velocity * DashSpeedFactor;
        player.Body3D.Velocity = dashVelocity;
    }

    public virtual void Shield(Player player, Direction direction)
    {
        GD.Print("Shield executed " + player.PlayerId + " towards " + direction);
    }
}
