using Godot;

public partial class Feature : Node
{
	protected Cooldown Cooldown => GetNode<Cooldown>("Cooldown");
	protected CharacterBody3D Body => GetNode<CharacterBody3D>("%PlayerBody");
	protected Player Player => GetParent<Player>();
}
