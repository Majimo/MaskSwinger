using Godot;

[GlobalClass]
public partial class AttackResource : Resource
{
    [Export] public string resourceName;
    [Export] public int damage;
    
    public virtual void Attack(player player)
    {
        GD.Print("Attack executed!");
    }
}
