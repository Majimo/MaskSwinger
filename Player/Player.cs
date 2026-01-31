using Godot;

public partial class Player : CharacterBody3D
{
	[Export] public int PlayerId { get; set; } = 1;
	
	[Export] public bool IsDashing { get; set; } 
	[Export] public bool IsShielding { get; set; } 
	[Export] public bool IsAttacking { get; set; } 
	
	public PlayerBehavior Behavior = new PlayerBehavior();

	public override void _PhysicsProcess(double delta)
	{
		if (!IsDashing)
		{
			var direction = Vector3.Zero;
			
			if (Input.IsActionPressed($"player_{this.PlayerId}_up"))
			{
				direction.Z = -1.0f;
			} 
			else if (Input.IsActionPressed($"player_{this.PlayerId}_down"))
			{
				direction.Z = 1.0f;
			}
			
			if (Input.IsActionPressed($"player_{this.PlayerId}_left"))
			{
				direction.X = -1.0f;
			}
			else if (Input.IsActionPressed($"player_{this.PlayerId}_right"))
			{
				direction.X = 1.0f;
			}

			if (direction != Vector3.Zero)
			{
				direction = direction.Normalized();
			}
			
			this.Velocity = direction * this.Behavior.Speed;
		}
		
		this.MoveAndSlide();
	}

	public void Hit()
	{
		this.ProcessMode = ProcessModeEnum.Disabled;
		this.Visible = false;
		
		this.Owner.ExecuteAfter(2, () =>
		{
			GD.Print("Respawn !!!");
			this.ProcessMode = ProcessModeEnum.Inherit;
			this.Visible = true;
		});
	}
}
