using System;
using Godot;
using Godot.Collections;

public partial class Lobby : Node
{
    private HBoxContainer JoinButtonsContainer => GetNodeOrNull<HBoxContainer>("MarginContainer/VBoxContainer/HBoxContainer");
    private Button StartButton => GetNodeOrNull<Button>("MarginContainer/VBoxContainer/MarginContainer/StartButton");
    private AudioStreamPlayer _lobbyStreamPlayer;
    private Array<Color> _playerColors =
    [
        Color.Color8(255, 0, 0),    // Red
        Color.Color8(0, 255, 0),    // Green
        Color.Color8(0, 0, 255),    // Blue
        Color.Color8(255, 255, 0)   // Yellow
    ];
    private AudioStream _playerJoinSound =
        GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Bloup.mp3");
    private AudioStream[] _startGameSounds =
    [
        GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Commencez_01.mp3"),
        GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Commencez_02.mp3"),
        GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Commencez_03.mp3"),
        GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Commencez_04.mp3")
    ];
    
    private readonly bool[] _playersJoined = new bool[4];

    public override void _Ready()
    {
        GameManager.Instance.ClearPlayers();
        
        for (var i = 0; i <= 3; i++)
        {
            var joinAvatar = JoinButtonsContainer.GetNode<TextureRect>($"Player{i}/MarginContainer/VBoxContainer/Avatar");
            joinAvatar.Modulate = Color.Color8(255, 255, 255, 75);
        }
        
        if (StartButton != null) StartButton.Disabled = true;
        
        _lobbyStreamPlayer = new AudioStreamPlayer();
        AddChild(_lobbyStreamPlayer);
    }

    public override void _Process(double delta)
    {
        for (var i = 0; i <= 3; i++)
        {
            if (_playersJoined[i]) continue; // Déjà rejoint
            
            if (Input.IsActionJustPressed($"player_{i}_join"))
            {
                JoinPlayer(i);
            }
        }

        for (var i = 0; i <= 3; i++)
        {
            if (!Input.IsActionJustPressed($"player_{i}_launch") ||
                GameManager.Instance.GetPlayerCount() <= 0) continue;
            StartGame();
            return;
        }
    }
    
    private void JoinPlayer(int playerId)
    {
        try
        {
            var joinButton = JoinButtonsContainer.GetNode<Button>($"Player{playerId}/MarginContainer/VBoxContainer/JoinButton");
            var joinAvatar = JoinButtonsContainer.GetNode<TextureRect>($"Player{playerId}/MarginContainer/VBoxContainer/Avatar");
            
            _playersJoined[playerId] = true;
            
            joinButton.Text = "Ready!";
            joinAvatar.Modulate = Color.Color8(255, 255, 255);
            
            _lobbyStreamPlayer.Stream = _playerJoinSound;
            _lobbyStreamPlayer.VolumeDb = 24;
            _lobbyStreamPlayer.Play();
            
            GameManager.Instance.AddPlayer(playerId, _playerColors[playerId]);
            
            if (StartButton != null)
            {
                StartButton.Disabled = false;
            }
        }
        catch (Exception e)
        {
            GD.PrintErr($"Erreur lors du join du joueur {playerId}: {e.Message}");
        }
    }
    
    private void StartGame()
    {
        var startSounds = _startGameSounds[Random.Shared.Next(_startGameSounds.Length)];
        _lobbyStreamPlayer.Stream = startSounds;
        _lobbyStreamPlayer.VolumeDb = 18;
        _lobbyStreamPlayer.Play();

        TimerUtils.ExecuteAfter(this, 2, () =>
        {
            GD.Print($"Starting game with {GameManager.Instance.GetPlayerCount()} players.");
            GetTree().ChangeSceneToFile("res://Battlefield/Battlefield.tscn");
        });
    }
}
