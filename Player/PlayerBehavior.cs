using Godot;
using MaskSwinger.Player;

[GlobalClass]
public partial class PlayerBehavior : Resource
{
    [Export] public int AttackDamage;
    [Export] public int AttackCooldown;
    [Export] public int ShieldDuration = 1;
    [Export] public int ShieldCooldown = 2;
    [Export] public float ShieldDivider = 2f;
    [Export] public int DashSpeedFactor = 3;
    [Export] public int DashCooldown = 2;
    [Export] public float DashDuration { get; set; } = 0.2f;
    
    public virtual void Attack(Player player, Direction direction)
    {
        GD.Print("Attacking " + player.PlayerId + " towards " + direction);
    }

    public virtual void Dash(Player player, Direction direction)
    {
        Vector3 dashVelocity = player.Body3D.Velocity * DashSpeedFactor;
        player.Body3D.Velocity = dashVelocity;
			
        Timer timer = new Timer();
        timer.WaitTime = player.Behavior.DashDuration;
        timer.OneShot = true;
        timer.Timeout += () =>
        {
            player.IsDashing = false;
            timer.QueueFree();
        };
        player.AddChild(timer);
        timer.Start();
    }

    public virtual void Shield(Player player, Direction direction)
    {
        float playerInitialSpeed = player.Speed;
        player.Speed = playerInitialSpeed / ShieldDivider;
        
        Timer timer = new Timer();
        timer.WaitTime = player.Behavior.ShieldDuration;
        timer.OneShot = true;
        timer.Timeout += () =>
        {
            player.IsShielding = false;
            player.Speed = playerInitialSpeed;
            timer.QueueFree();
        };
        player.AddChild(timer);
        timer.Start();
    }
}
