using Godot;

[GlobalClass]
public partial class PlayerBehavior : Resource
{
    [Export] public int AttackDamage;
    [Export] public int AttackCooldown;
    [Export] public int ShieldDuration;
    [Export] public int ShieldCooldown;
    [Export] public int DashRange;
    [Export] public int DashCooldown;
    
    public enum DirectionEnum
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    
    public virtual void Attack(Player player, DirectionEnum Direction)
    {
        GD.Print("Attacking " + player.PlayerId + " towards " + Direction);
    }

    public virtual void Dash(Player player, DirectionEnum Direction)
    {
        GD.Print("Dash executed " + player.PlayerId + " towards " + Direction);
    }

    public virtual void Shield(Player player, DirectionEnum Direction)
    {
        GD.Print("Shield executed " + player.PlayerId + " towards " + Direction);
    }
}
