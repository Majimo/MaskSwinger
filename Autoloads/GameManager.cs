using Godot;
using System.Collections.Generic;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    
    public List<PlayerData> JoinedPlayers { get; private set; } = new();
    
    public override void _EnterTree()
    {
        Instance = this;
    }
    
    public override void _ExitTree()
    {
        Instance = null;
    }
    
    public void ClearPlayers()
    {
        JoinedPlayers.Clear();
    }
    
    public void AddPlayer(int playerId, Color playerColor)
    {
        JoinedPlayers.Add(new PlayerData
        {
            PlayerId = playerId,
            Color = playerColor,
            IsJoined = true
        });
    }
    
    public bool IsPlayerJoined(int playerId)
    {
        return JoinedPlayers.Exists(p => p.PlayerId == playerId);
    }
    
    public int GetPlayerCount()
    {
        return JoinedPlayers.Count;
    }

    public static void Killing(Player killed, Player killer)
    {
        GD.Print($"Player {killed.PlayerId} killed by player {killer.PlayerId}");
    }
}

public class PlayerData
{
    public int PlayerId { get; set; }
    public Color Color { get; set; }
    public bool IsJoined { get; set; }
}