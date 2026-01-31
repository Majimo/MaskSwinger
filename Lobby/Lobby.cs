using System;
using Godot;
using Godot.Collections;

public partial class Lobby : Node
{
    private HBoxContainer JoinButtonsContainer => GetNodeOrNull<HBoxContainer>("MarginContainer/VBoxContainer/HBoxContainer");
    private Button StartButton => GetNodeOrNull<Button>("MarginContainer/VBoxContainer/MarginContainer/StartButton");
    
    private Array<Color> _playerColors =
    [
        Color.Color8(255, 0, 0, 255),    // Red
        Color.Color8(0, 255, 0, 255),    // Green
        Color.Color8(0, 0, 255, 255),    // Blue
        Color.Color8(255, 255, 0, 255)   // Yellow
    ];
    private bool _atLeastOnePlayerJoined = false;

    public override void _Ready()
    {
        for (var i = 1; i <= 4; i++)
        {
            var joinAvatar = JoinButtonsContainer.GetNode<TextureRect>($"Player{i}/VBoxContainer/Avatar");
            joinAvatar.Modulate = Color.Color8(255, 255, 255, 75);
        }
    }

    public override void _Process(double delta)
    {
        try
        {
            for (var i = 1; i <= 4; i++)
            {
                var joinButton = JoinButtonsContainer.GetNode<Button>($"Player{i}/VBoxContainer/JoinButton");
                var joinAvatar = JoinButtonsContainer.GetNode<TextureRect>($"Player{i}/VBoxContainer/Avatar");
                
                if (!Input.IsActionJustPressed($"player_{i}_join")) continue;
                
                joinButton.Text = "Joined";
                joinAvatar.Modulate = _playerColors[i - 1];
                _atLeastOnePlayerJoined = true;
                StartButton.Disabled = !_atLeastOnePlayerJoined;
            }
        }
        catch (Exception e)
        {
            GD.PrintErr($"Erreur : {e.Message}");
        }   

        for (var i = 1; i <= 4; i++)
        {
            if (!Input.IsActionJustPressed($"player_{i}_launch") || !_atLeastOnePlayerJoined) continue;
            
            GetTree().ChangeSceneToFile("res://Battlefield/Battlefield.tscn");
            return;
        }
    }
}
