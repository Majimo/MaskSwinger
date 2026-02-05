using System;
using Godot;
using Godot.Collections;

public partial class Battlefield : Node3D
{
    private System.Collections.Generic.Dictionary<int, VBoxContainer> PlayerLeaderBoards { get; set; } = [];
    private PackedScene PlayerScene { get; set; } = GD.Load<PackedScene>("res://Player/Player.tscn");
    
    private AudioStreamPlayer _musicPlayer;
    private AudioStreamPlayer _countDownPlayer2;
    private AudioStream[] _countDownSounds = 
    [
        GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Decompte_01.mp3"),
        GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Decompte_02.mp3"),
        GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Decompte_03.mp3"),
        GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Decompte_04.mp3")
    ];
    private Vector3[] _spawnPoints = 
    [
        new Vector3(-30, 0, -30),
        new Vector3(30, 0, -30),
        new Vector3(-30, 0, 30),
        new Vector3(30, 0, 30)
    ];
    private TimeSpan _timeRemaining = TimeSpan.FromSeconds(141); // 2min24s = 144 secondes
    private bool _isRunning = true;
    
    public override void _Ready()
    {
        var players = SpawnPlayers();

        var phantomCamera = GetNode("PhantomCamera3D");
        
        phantomCamera.Call("set_follow_targets", players);
        
        _musicPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        _musicPlayer.Finished += OnMusicFinished;
    }
    
    public override void _Process(double delta)
    {
        foreach (var player in PlayerLeaderBoards)
        {
            player.Value.GetNode<Label>("Kills/Count").Text = GameManager.Instance.JoinedPlayers[player.Key].LeaderBoardEntry.Kills.ToString();
            player.Value.GetNode<Label>("Deaths/Count").Text = GameManager.Instance.JoinedPlayers[player.Key].LeaderBoardEntry.Deaths.ToString();
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
    
    private Array<Node3D> SpawnPlayers()
    {
        var players = new Array<Node3D>();
        
        foreach (var playerData in GameManager.Instance.JoinedPlayers.Values)
        {
            players.Add(SpawnPlayer(playerData));
        }

        return players;
    }
    
    private Player SpawnPlayer(PlayerData playerData)
    {
        var playerInstance = PlayerScene.Instantiate<Player>();
        
        AddChild(playerInstance);
        
        playerInstance.GlobalPosition = _spawnPoints[playerData.PlayerId];
        playerInstance.PlayerId = playerData.PlayerId;
        
        var playerLeadBoard = GetNode<VBoxContainer>($"UIDeathKills/Player{playerData.PlayerId}LeadBoard");
        playerLeadBoard.Visible = true;
        PlayerLeaderBoards[playerData.PlayerId] = playerLeadBoard;
        
        return playerInstance;
    }

    private void OnMusicFinished()
    {
        GetNode<AudioStreamPlayer>("FinDuTempsPlayer").Play();
        
        this.ExecuteAfter(1.0f , () =>
        {
            GetNode<Control>("End").Visible = true;
            PlayerData winningPlayerData = GameManager.Instance.GetTopPlayer();
            GetNode<Label>("End/VBoxContainer/Winner").Text = 
                $"Player {winningPlayerData.PlayerId + 1} avec {winningPlayerData.LeaderBoardEntry.Kills} kills et {winningPlayerData.LeaderBoardEntry.Deaths} morts";
            GetTree().Paused = true;
        });
    }
}
