using Godot;

[GlobalClass]
public partial class SwordMaskAttack : AttackResource
{
    [Export] public int slashRange = 50;
    
    public override void Attack(player player)
    {
        GD.Print($"Player {player.PlayerId} performs a sword mask attack!");
    }
}
