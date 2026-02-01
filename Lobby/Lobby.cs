using System;
using Godot;
using Godot.Collections;

public partial class Lobby : Node
{
    private HBoxContainer JoinButtonsContainer => GetNodeOrNull<HBoxContainer>("MarginContainer/VBoxContainer/HBoxContainer");
    private Button StartButton => GetNodeOrNull<Button>("MarginContainer/VBoxContainer/MarginContainer/StartButton");
    
    private Array<Color> _playerColors =
    [
        Color.Color8(255, 0, 0),    // Red
        Color.Color8(0, 255, 0),    // Green
        Color.Color8(0, 0, 255),    // Blue
        Color.Color8(255, 255, 0)   // Yellow
    ];
    
    private readonly bool[] _playersJoined = new bool[4];

    public override void _Ready()
    {
        GameManager.Instance.ClearPlayers();
        
        for (var i = 0; i <= 3; i++)
        {
            var joinAvatar = JoinButtonsContainer.GetNode<TextureRect>($"Player{i}/VBoxContainer/Avatar");
            joinAvatar.Modulate = Color.Color8(255, 255, 255, 75);
        }
        if (StartButton != null) StartButton.Disabled = true;
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
            var joinButton = JoinButtonsContainer.GetNode<Button>($"Player{playerId}/VBoxContainer/JoinButton");
            var joinAvatar = JoinButtonsContainer.GetNode<TextureRect>($"Player{playerId}/VBoxContainer/Avatar");
            
            _playersJoined[playerId] = true;
            
            joinButton.Text = "Ready!";
            joinButton.Disabled = true;
            joinAvatar.Modulate = _playerColors[playerId];
            
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
        GD.Print($"Starting game with {GameManager.Instance.GetPlayerCount()} players.");
        GetTree().ChangeSceneToFile("res://Battlefield/Battlefield.tscn");
    }
}
