using Godot;
using MaskSwinger.Player;

[GlobalClass]
public partial class BullBehavior : PlayerBehavior
{
    [Export] public int slashRange = 50;
    
    public override void Attack(Player player, Direction direction)
    {
        GD.Print($"Player {player.PlayerId} performs a sword mask attack!");
    }
}
