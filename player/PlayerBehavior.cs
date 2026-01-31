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
    
    public virtual void Attack(Player player)
    {
        if (Input.IsActionJustPressed("player_" + player.PlayerId + "_attack_up"))
        {
            GD.Print("Player " + player.PlayerId + " attacked UP");
        }
        if (Input.IsActionJustPressed("player_" + player.PlayerId + "_attack_down"))
        {
            GD.Print("Player " + player.PlayerId + " attacked with damage DOWN ");
        }
        if (Input.IsActionJustPressed("player_" + player.PlayerId + "_attack_left"))
        {
            GD.Print("Player " + player.PlayerId + " attacked with damage LEFT ");
        }
        if (Input.IsActionJustPressed("player_" + player.PlayerId + "_attack_right"))
        {
            GD.Print("Player " + player.PlayerId + " attacked with damage RIGHT ");
        }
    }

    public virtual void Dash(Player player)
    {
        GD.Print("Dash executed!");
    }

    public virtual void Shield(Player player)
    {
        GD.Print("Shield executed!");
    }
}
