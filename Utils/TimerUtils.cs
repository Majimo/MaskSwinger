using System;
using Godot;

public static class TimerUtils
{
    public static void ExecuteAfter(this Node target, double duration, Action onTimeout)
    {
        var timer = new Timer
        {
            WaitTime = duration,
            OneShot = true
        };
		
        timer.Timeout += onTimeout;
        timer.Timeout += () =>
        {
            timer.QueueFree();
        };
		
        target.AddChild(timer);
        timer.Start();
    }
}