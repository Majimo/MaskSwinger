using Godot;

[GlobalClass]
public partial class BullBehavior : PlayerBehavior
{
    [Export] public int slashRange = 50;
    
    public override void Attack(Player player, DirectionEnum Direction)
    {
        GD.Print($"Player {player.PlayerId} performs a sword mask attack!");
    }
}
