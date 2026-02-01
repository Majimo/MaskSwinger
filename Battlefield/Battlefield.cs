using Godot;
using System.Collections.Generic;

public partial class Battlefield : Node3D
{
    private PackedScene PlayerScene { get; set; } = GD.Load<PackedScene>("res://Player/Player.tscn");
    private Node _phantomCamera;
    
    private List<Node3D> _playerNodes = new List<Node3D>();
    
    private Vector3[] _spawnPoints = new Vector3[]
    {
        new Vector3(-30, 0, -30),
        new Vector3(30, 0, -30),
        new Vector3(-30, 0, 30),
        new Vector3(30, 0, 30)
    };
    
    public override void _Ready()
    {
        _phantomCamera = GetNode("PhantomCamera3D");
        
        GD.Print("=== Battlefield Ready ===");
        GD.Print($"PhantomCamera found: {_phantomCamera != null}");
        
        SpawnPlayers();
        
        CallDeferred(nameof(ConfigureCamera));
    }

    private void SpawnPlayers()
    {
        List<PlayerData> joinedPlayers = GameManager.Instance.JoinedPlayers;
        
        GD.Print($"Spawning {joinedPlayers.Count} players");
        
        foreach (PlayerData playerData in joinedPlayers)
        {
            SpawnPlayer(playerData);
        }
    }
    
    private void SpawnPlayer(PlayerData playerData)
    {
        var playerInstance = PlayerScene.Instantiate<Node3D>();
        
        int spawnIndex = playerData.PlayerId;
        
        AddChild(playerInstance);
        
        playerInstance.GlobalPosition = _spawnPoints[spawnIndex];
        
        if (playerInstance is Player player)
        {
            player.PlayerId = playerData.PlayerId;
            
            _playerNodes.Add(playerInstance);
            
            GD.Print($"✅ Spawned Player {playerData.PlayerId} at {_spawnPoints[spawnIndex]}");
        }
    }
    
    private void ConfigureCamera()
    {
        if (_phantomCamera == null)
        {
            GD.PrintErr("PhantomCamera not found!");
            return;
        }
        
        if (_playerNodes.Count == 0)
        {
            GD.PrintErr("No players to follow!");
            return;
        }
        
        GD.Print($"🎥 Configuring camera with {_playerNodes.Count} targets");
        
        var godotArray = new Godot.Collections.Array<Node3D>(_playerNodes);
        
        foreach (var player in _playerNodes)
        {
            GD.Print($"  Added target: {player.Name} at {player.GlobalPosition}");
        }
        
        if (_playerNodes.Count > 1)
        {
            _phantomCamera.Set("follow_mode", 3);
            _phantomCamera.Call("set_follow_targets", godotArray);
            
            _phantomCamera.Set("follow_offset", new Vector3(0, 25, 35));
            
            _phantomCamera.Set("auto_follow_distance", true);
            _phantomCamera.Set("auto_follow_distance_min", 15.0f);  // Zoom max (joueurs proches)
            _phantomCamera.Set("auto_follow_distance_max", 50.0f);  // Zoom min (joueurs éloignés)
            _phantomCamera.Set("auto_follow_distance_divisor", 8.0f); // ⭐ Sensibilité du zoom
            
            _phantomCamera.Set("follow_damping", true);
            _phantomCamera.Set("follow_damping_value", new Vector3(3.0f, 3.0f, 3.0f));
            
            _phantomCamera.Set("dead_zone_width", 0.05f);
            _phantomCamera.Set("dead_zone_height", 0.05f);
            
            GD.Print("✅ Camera: FRAMED mode configured");
            GD.Print($"   Min distance: 15, Max distance: 50, Divisor: 8");
        }
        else
        {
            _phantomCamera.Set("follow_mode", 2); // Simple
            _phantomCamera.Call("set_follow_target", _playerNodes[0]);
            
            _phantomCamera.Set("follow_offset", new Vector3(0, 15, 25));
            _phantomCamera.Set("follow_damping", true);
            _phantomCamera.Set("follow_damping_value", new Vector3(2.0f, 2.0f, 2.0f));
            _phantomCamera.Set("auto_follow_distance", false);
            
            GD.Print("✅ Camera: SIMPLE mode configured");
        }
        GD.Print($"📸 Follow offset: {_phantomCamera.Get("follow_offset")}");
        GD.Print($"📸 Auto distance: {_phantomCamera.Get("auto_follow_distance")}");
        GD.Print($"📸 Distance divisor: {_phantomCamera.Get("auto_follow_distance_divisor")}");
    }

    
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            DebugCamera();
        }
    }
    
    private void DebugCamera()
    {
        GD.Print("=== 🎥 Camera Debug ===");
        GD.Print($"Camera node: {_phantomCamera}");
        GD.Print($"Follow mode: {_phantomCamera.Get("follow_mode")}");
        GD.Print($"Follow offset: {_phantomCamera.Get("follow_offset")}");
        GD.Print($"Follow targets: {_phantomCamera.Get("follow_targets")}");
        GD.Print($"Auto distance: {_phantomCamera.Get("auto_follow_distance")}");
        GD.Print($"Player nodes: {_playerNodes.Count}");
        
        foreach (var player in _playerNodes)
        {
            GD.Print($"  - {player.Name} at {player.GlobalPosition}");
        }
        GD.Print("====================");
    }
}
