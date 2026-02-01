using Godot;
using System.Collections.Generic;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    
    public List<PlayerData> JoinedPlayers { get; private set; } = new();
    
    private AudioStreamPlayer _killingSpreePlayer;
    private PlayerData _lastKiller;
    private int _killingSpreeCount = 0;

    public override void _Ready()
    {
        base._Ready();
    }

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
            IsJoined = true,
            LeaderBoardEntry = new PlayerLeaderBoardEntry
            {
                Kills = 0,
                Deaths = 0
            }
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

    public PlayerData GetTopPlayer()
    {
        PlayerData topPlayer = null;
        foreach (var player in JoinedPlayers)
        {
            if (topPlayer == null ||
                player.LeaderBoardEntry.Kills > topPlayer.LeaderBoardEntry.Kills ||
                (player.LeaderBoardEntry.Kills == topPlayer.LeaderBoardEntry.Kills &&
                 player.LeaderBoardEntry.Deaths < topPlayer.LeaderBoardEntry.Deaths))
            {
                topPlayer = player;
            }
        }
        return topPlayer;
    }

    public void Killing(Player killed, Player killer)
    {
        if (_lastKiller == null)
        {
            // Play firstKill
            _killingSpreeCount = 1;
        } else if (_lastKiller.PlayerId == killer.PlayerId)
        {
            _killingSpreeCount ++;
            if (_killingSpreeCount == 2)
            {
                // Play doubleKill
            } else if (_killingSpreeCount == 5)
            {
                // Meurtre de masse
            }
        } else
        {
            _killingSpreeCount = 1;
        }
        _lastKiller = JoinedPlayers[killer.PlayerId];
        JoinedPlayers[killer.PlayerId].LeaderBoardEntry.Kills++;
        JoinedPlayers[killed.PlayerId].LeaderBoardEntry.Deaths++;
    }
}

public class PlayerLeaderBoardEntry
{
    public int Kills { get; set; }
    public int Deaths { get; set; }
}

public class PlayerData
{
    public int PlayerId { get; set; }
    public Color Color { get; set; }
    public bool IsJoined { get; set; }
    public PlayerLeaderBoardEntry LeaderBoardEntry { get; set; }
}