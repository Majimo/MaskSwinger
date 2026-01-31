using Godot;
using MaskSwinger.Player;

[GlobalClass]
public partial class PlayerBehavior : Resource
{
    [Export] public int AttackDamage;
    [Export] public int AttackCooldown;
    [Export] public int ShieldDuration;
    [Export] public int ShieldCooldown;
    [Export] public int DashRange;
    [Export] public int DashCooldown;
    
    public virtual void Attack(Player player, Direction direction)
    {
        GD.Print("Attacking " + player.PlayerId + " towards " + direction);
    }

    public virtual void Dash(Player player, Direction direction)
    {
        GD.Print("Dash executed " + player.PlayerId + " towards " + direction);
        player.IsDashing = true;
    }

    public virtual void Shield(Player player, Direction direction)
    {
        GD.Print("Shield executed " + player.PlayerId + " towards " + direction);
    }
}
