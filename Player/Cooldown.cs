using Godot;
using System;

[GlobalClass]
public partial class Cooldown : Node
{
    [Export] public double Duration = 1.0d;

    private double _current;
    
    public override void _Process(double delta)
    {
        if (_current > 0) _current -= delta;
    }

    public bool CanExecute()
    {
        return _current <= 0;
    }

    public void Restart()
    {
        _current = Duration;
    }
}
