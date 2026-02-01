using Godot;

public partial class MaskSpawner : Node3D
{
    private PackedScene _maskScene;
    private Mask _currentMask;

    public override void _Ready()
    {
        _maskScene = GD.Load<PackedScene>("res://Mask/Mask.tscn");
        
        this.InstantiateMask();
        
        this.GetNode<Timer>("Timer").Timeout += TimerOnTimeout;
    }

    private void TimerOnTimeout()
    {
        if (_currentMask is null && GD.Randf() < 0.5f)
        {
            this.InstantiateMask();
        }
    }

    private void InstantiateMask()
    {
        _currentMask = _maskScene.Instantiate<Mask>();
        _currentMask.Position = Position with { Z = 1 };
        AddChild(_currentMask);
    }
}
