using Godot;

[GlobalClass]
public partial class PlayerBehavior : Resource
{
    [Export] public int Damage;
    [Export] public int Shield;
    [Export] public int Dash;
    
    public virtual void Attack(Player player)
    {
        GD.Print("Attack executed!");
    }
}
