using Godot;

public partial class Feature : Node
{
	protected Cooldown Cooldown => GetNode<Cooldown>("Cooldown");
	protected CharacterBody3D Body => GetNode<CharacterBody3D>("%PlayerBody");
	protected Player Player => GetParent<Player>();
	protected Area3D AttackZoneLeft => GetNode<Area3D>("%AttackZoneLeft");
	protected Area3D AttackZoneRight => GetNode<Area3D>("%AttackZoneRight");
	protected Area3D AttackZoneUp => GetNode<Area3D>("%AttackZoneUp");
	protected Area3D AttackZoneDown => GetNode<Area3D>("%AttackZoneDown");
}
