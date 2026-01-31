using Godot;

public partial class MaskSpawner : Node3D
{
    private PackedScene _maskScene;
    private Mask _currentMask;

    public override void _Ready()
    {
        _maskScene = GD.Load<PackedScene>("res://Mask/Mask.tscn");
        this.GetNode<Timer>("Timer").Timeout += TimerOnTimeout;
    }

    private void TimerOnTimeout()
    {
        if (this._currentMask is null && GD.Randf() < 0.5f)
        {
            this._currentMask = _maskScene.Instantiate<Mask>();
            AddChild(this._currentMask);
        }
    }
}
