using System;
using Godot;
using System.Collections.Generic;

public partial class Battlefield : Node3D
{
    private List<VBoxContainer> PlayerLeaderBoards { get; set; } = new List<VBoxContainer>();
    private PackedScene PlayerScene { get; set; } = GD.Load<PackedScene>("res://Player/Player.tscn");
    private Node _phantomCamera;
    private List<Node3D> _playerNodes = new List<Node3D>();
    private AudioStreamPlayer _musicPlayer;
    private AudioStreamPlayer _countDownPlayer2;
    private AudioStream[] _countDownSounds = new AudioStream[]
    {
        GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Decompte_01.mp3"),
        GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Decompte_02.mp3"),
        GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Decompte_03.mp3"),
        GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Decompte_04.mp3")
    };
    private Vector3[] _spawnPoints = new Vector3[]
    {
        new Vector3(-30, 0, -30),
        new Vector3(30, 0, -30),
        new Vector3(-30, 0, 30),
        new Vector3(30, 0, 30)
    };
    private TimeSpan _timeRemaining = TimeSpan.FromSeconds(141); // 2min24s = 144 secondes
    private bool _isRunning = true;
    
    public override void _Ready()
    {
        _phantomCamera = GetNode("PhantomCamera3D");
        
        SpawnPlayers();
        
        CallDeferred(nameof(ConfigureCamera));
        
        if (_playerNodes.Count is <= 4 and > 0)
        {
            for (int i = 0; i < _playerNodes.Count; i++)
            {
                VBoxContainer playerLeadBoard = GetNode<VBoxContainer>($"UIDeathKills/Player{i + 1}LeadBoard");
                playerLeadBoard.Visible = true;
                PlayerLeaderBoards.Add(playerLeadBoard);
            }
        }
        
        _musicPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        _musicPlayer.Finished += OnMusicFinished;
    }
    
    public override void _Process(double delta)
    {
        // if (Input.IsActionJustPressed("ui_accept"))
        // {
        //     DebugCamera();
        // }
        foreach (var player in PlayerLeaderBoards)
        {
            player.GetNode<Label>("Kills/Count").Text = GameManager.Instance.JoinedPlayers[PlayerLeaderBoards.IndexOf(player)].LeaderBoardEntry.Kills.ToString();
            player.GetNode<Label>("Deaths/Count").Text = GameManager.Instance.JoinedPlayers[PlayerLeaderBoards.IndexOf(player)].LeaderBoardEntry.Deaths.ToString();
        }
        
        if (!_isRunning) return;
        
        _timeRemaining = _timeRemaining.Add(-TimeSpan.FromSeconds(delta));
        GetNode<Label>("TimeRemaining/Label").Text = _timeRemaining.ToString(@"mm\:ss");

        if (_timeRemaining.TotalSeconds <= 5)
        {
            GetNode<Label>("TimeRemaining/Label2").Visible = true;
            GetNode<Label>("TimeRemaining/Label2").Text = ((int)_timeRemaining.TotalSeconds).ToString();
            // if (_timeRemaining % 1 == 0)
            // {
            //     _countDownPlayer2.Stream = _countDownSounds[(int)_timeRemaining - 1];
            //     _countDownPlayer2.VolumeDb = 24;
            //     _countDownPlayer2.Play();
            // }
        }
        
        if (_timeRemaining.TotalSeconds <= 0)
        {
            GetNode<Label>("TimeRemaining/Label2").Visible = false;
            _timeRemaining = TimeSpan.Zero;
            _isRunning = false;
        }
    }
    
    private string FormatTime(float seconds)
    {
        int minutes = (int)(seconds / 60);
        int secs = (int)(seconds % 60);
        return $"{minutes}:{secs:D2}";
    }

    private void SpawnPlayers()
    {
        List<PlayerData> joinedPlayers = GameManager.Instance.JoinedPlayers;
        
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
        }
    }

    private void OnMusicFinished()
    {
        GetNode<AudioStreamPlayer>("FinDuTempsPlayer").Play();
        this.ExecuteAfter(1.0f , () =>
        {
            GetNode<Control>("End").Visible = true;
            PlayerData winningPlayerData = GameManager.Instance.GetTopPlayer();
            GetNode<Label>("End/VBoxContainer/Winner").Text = 
                $"Player {winningPlayerData.PlayerId} avec {winningPlayerData.LeaderBoardEntry.Kills} kills et {winningPlayerData.LeaderBoardEntry.Deaths} morts";
            GetTree().Paused = true;
        });
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
        }
        else
        {
            _phantomCamera.Set("follow_mode", 2); // Simple
            _phantomCamera.Call("set_follow_target", _playerNodes[0]);
            
            _phantomCamera.Set("follow_offset", new Vector3(0, 15, 25));
            _phantomCamera.Set("follow_damping", true);
            _phantomCamera.Set("follow_damping_value", new Vector3(2.0f, 2.0f, 2.0f));
            _phantomCamera.Set("auto_follow_distance", false);
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
