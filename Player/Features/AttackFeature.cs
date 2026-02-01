using System;
using System.Diagnostics.CodeAnalysis;
using Godot;
using MaskSwinger.Player;

[GlobalClass]
public partial class AttackFeature : Feature
{
    protected override void DoFeature(double delta)
    {
        if (!GetAttackVector(out var direction))
        {
            return;
        }
        
        this.Player.Behavior.Attack(this.Player, direction.Value);
        this.Player.IsAttacking = true;
        this.Cooldown.Restart(this.Player.Behavior.AttackCooldown);
    }
    
    private bool GetAttackVector([NotNullWhen(true)]out Direction? direction)
    {
        direction = null;
        
        if (Input.IsActionPressed($"player_{this.Player.PlayerId}_attack_up"))
        {
            direction = Direction.Up;
        } 
        else if (Input.IsActionPressed($"player_{this.Player.PlayerId}_attack_down"))
        {
            direction = Direction.Down;
        } 
        else if(Input.IsActionPressed($"player_{this.Player.PlayerId}_attack_left"))
        {
            direction = Direction.Left;
        }
        else if (Input.IsActionPressed($"player_{this.Player.PlayerId}_attack_right"))
        {
            direction = Direction.Right;
        }
        
        return direction != null;
    }
}
