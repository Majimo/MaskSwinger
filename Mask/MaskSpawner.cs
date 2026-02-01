using System;
using Godot;

[GlobalClass]
public partial class MaskSpawner : Node3D
{
    private PackedScene _maskScene;
    private Mask _currentMask;
    
    private PlayerBehavior[] _maskFactory;

    [Export] public float SpawnRate { get; set; } = 1f;
    [Export] public float SpawnInterval { get; set; } = 10f;

    public override void _Ready()
    {
        _maskScene = GD.Load<PackedScene>("res://Mask/Mask.tscn");

        _maskFactory =
        [
            GD.Load<BullBehavior>("res://Mask/Bull/BullBehavior.tres"),
            GD.Load<ChameleonBehavior>("res://Mask/Chameleon/ChameleonBehavior.tres"),
            GD.Load<ErmineBehavior>("res://Mask/Ermine/ErmineBehavior.tres"),
            GD.Load<HippopotamusBehavior>("res://Mask/Hippopotamus/HippopotamusBehavior.tres"),
            GD.Load<OwlBehavior>("res://Mask/Owl/OwlBehavior.tres"),
            GD.Load<RamBehavior>("res://Mask/Ram/RamBehavior.tres")
        ];
            
        this.InstantiateMask();

        var timer = new Timer
        {
            Autostart = true,
            WaitTime = SpawnInterval,
        };
        
        timer.Timeout += TimerOnTimeout;
        
        this.AddChild(timer);
    }

    private void TimerOnTimeout()
    {
        if (_currentMask is null && GD.Randf() < this.SpawnInterval)
        {
            this.InstantiateMask();
        }
    }

    private void InstantiateMask()
    {
        var maskBehavior = GetRandomMask();
        
        _currentMask = _maskScene.Instantiate<Mask>();
        
        GD.Print("Setting Behavior");
        
        _currentMask.Position = Position with { Z = Position.Z + 1 };
        _currentMask.Behavior = maskBehavior;
        _currentMask.Spawner = this;
        
        AddChild(_currentMask);
    }
    
    public void ReleaseMask()
    {
        _currentMask = null;
    }

    private PlayerBehavior GetRandomMask()
    {
        var maskId = Random.Shared.Next(_maskFactory.Length);

        var mask = (PlayerBehavior)_maskFactory[maskId].Duplicate();
        
        GD.Print($"Generated mask {mask.GetType()} ({mask.MaskColor})");

        return mask;
    }
}
