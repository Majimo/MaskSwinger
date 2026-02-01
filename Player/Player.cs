using Godot;

public partial class Player : CharacterBody3D
{
	private const float DeadZone = 0.2f;
	
	[Export] public int PlayerId { get; set; }
	
	[Export] public bool IsDashing { get; set; } 
	[Export] public bool IsShielding { get; set; } 
	[Export] public bool IsAttacking { get; set; } 
	
	public PlayerBehavior Behavior = new PlayerBehavior();

	public override void _PhysicsProcess(double delta)
	{
		if (!IsDashing)
		{
			var direction = Vector3.Zero;

			var xAxis = Input.GetJoyAxis(this.PlayerId, JoyAxis.LeftX);
			if (Mathf.Abs(xAxis) > DeadZone)
			{
				direction.X = xAxis;
			} 
			
			var yAxis = Input.GetJoyAxis(this.PlayerId, JoyAxis.LeftY);
			if (Mathf.Abs(yAxis) > DeadZone)
			{
				direction.Z = yAxis;
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
