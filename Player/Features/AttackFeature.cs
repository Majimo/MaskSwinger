using System;
using Godot;

[GlobalClass]
public partial class AttackFeature : Feature
{
    protected override void DoFeature(double delta)
    {
        if (!GetAttackVector(out var direction))
        {
            return;
        }
        
        this.Player.Behavior.Attack(this.Player, direction);
        this.Player.IsAttacking = true;
        this.Cooldown.Restart(this.Player.Behavior.AttackCooldown);
    }

    private bool GetAttackVector(out Vector3 direction)
    {
        direction = Vector3.Zero;
        
        if (Input.IsActionPressed($"player_{this.Player.PlayerId}_attack_up"))
        {
            direction.Z = -1.0f;
        } 
        else if (Input.IsActionPressed($"player_{this.Player.PlayerId}_attack_down"))
        {
            direction.Z = 1.0f;
        }
			
        if (Input.IsActionPressed($"player_{this.Player.PlayerId}_attack_left"))
        {
            direction.X = -1.0f;
        }
        else if (Input.IsActionPressed($"player_{this.Player.PlayerId}_attack_right"))
        {
            direction.X = 1.0f;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
        }
        
        return direction != Vector3.Zero;
    }
    
    // private Dictionary<Area3D, Direction> _attackZones;
    
    // public override void _Ready()
    // {
    //     _attackZones = new Dictionary<Area3D, Direction>
    //     {
    //         { AttackZoneLeft, Direction.Left },
    //         { AttackZoneRight, Direction.Right },
    //         { AttackZoneUp, Direction.Up },
    //         { AttackZoneDown, Direction.Down }
    //     };
    //
    //     foreach (var keyValuePair in _attackZones)
    //     {
    //         Area3D zone = keyValuePair.Key;
    //         Direction direction = keyValuePair.Value;
    //     }
    // }
    
    // public override void _PhysicsProcess(double delta)
    // {
    //     if (this.Cooldown.CanExecute())
    //     {
    //         foreach (var keyValuePair in _attackZones)
    //         {
    //             Area3D zone = keyValuePair.Key;
    //             Direction direction = keyValuePair.Value;
    //
    //             if (Input.IsActionJustPressed("player_" + this.Player.PlayerId + "_attack_" + direction))
    //             {
    //                 GD.Print($"Player {this.Player.PlayerId} attacks {direction} from zone {zone}");
    //                 ExecuteAttack(direction);
    //                 // var hits = zone.GetOverlappingBodies().OfType<CharacterBody3D>();
    //                 //
    //                 // foreach (var overlappingBody in hits)
    //                 // {
    //                 //     if (overlappingBody.GetParent() is Player player)
    //                 //     {
    //                 //         GD.Print($"Player {this.Player.PlayerId} overlapping with Player {player.PlayerId}");
    //                 //         Player hitPlayer = player.PlayerId != this.Player.PlayerId ? player : null;
    //                 //         if (hitPlayer != null)
    //                 //         {
    //                 //             GD.Print($"Player {this.Player.PlayerId} HIT Player {hitPlayer.PlayerId} from {direction}");
    //                 //             OnPlayerHit(hitPlayer, direction);
    //                 //         }
    //                 //     }
    //                 // }
    //             }
    //         }
    //     }
    // }

    // private void ExecuteAttack(Direction direction)
    // {
    //     GD.Print($"Player {this.Player.PlayerId} attacks {direction}");
    //     this.Player.Behavior.Attack(this.Player, direction);
    //     this.Cooldown.Restart();
    //         
    // }
    
    // private void OnAttackZoneBodyEntered(Node3D body, Direction attackDirection)
    // {
    //     Player otherPlayer = GetPlayerFromBody(body);
    //     GD.Print($"Player {this.Player.PlayerId} hit OnAttackZoneBodyEntered {otherPlayer?.PlayerId}");
    //     
    //     if (otherPlayer != null)
    //     {
    //         GD.Print($"Player {this.Player.PlayerId} HIT Player {otherPlayer.PlayerId} from {attackDirection}");
    //         OnPlayerHit(otherPlayer, attackDirection);
    //     }
    // }
    
    // private void OnPlayerHit(Player hitPlayer, Direction attackDirection)
    // {
    //     int damage = this.Player.Behavior?.AttackDamage ?? 10;
    //     GD.Print("Player " + hitPlayer.PlayerId + " takes " + damage + " damage!");
    //     ExecuteAttack(attackDirection);
    // }
    
    // private Player GetPlayerFromBody(Node3D body)
    // {
    //     Node current = body;
    //     GD.Print($"Player {this.Player.PlayerId} hit BODY {body}");
    //     
    //     while (current != null)
    //     {
    //         if (current is Player player)
    //         {
    //             GD.Print($"Player {this.Player.PlayerId} hit PLAYER {player.PlayerId}");
    //             // Ne pas se détecter soi-même
    //             return player.PlayerId != this.Player.PlayerId ? player : null;
    //         }
    //         current = current.GetParent();
    //     }
    //     
    //     return null;
    // }
}
