using System;
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }

    public Dictionary<int, PlayerData> JoinedPlayers { get; private set; } = [];
    
    private AudioStreamPlayer _killingSpreePlayer;
    private PlayerData _lastKiller;
    private int _killingSpreeCount = 0;

    private AudioStream[] _firstBloodSounds;
    private AudioStream[] _doubleDeathSounds;
    private AudioStream[] _killingSpreeSounds;

    public override void _Ready()
    {
        _killingSpreePlayer = new AudioStreamPlayer();
        _firstBloodSounds = [
            GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Premier_Meutre_01.mp3"),
            GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Premier_Meutre_02.mp3"),
            GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Premier_Meutre_03.mp3")
        ];
        _doubleDeathSounds = [
            GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Double_deces_01.mp3"),
            GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Double_deces_02.mp3")
        ];
        _killingSpreeSounds = [
            GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Morts_de_masse_01.mp3"),
            GD.Load<AudioStream>("res://AudioAssets/SoundEffects/Morts_de_masse_02.mp3")
        ];
        AddChild(_killingSpreePlayer);
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
        JoinedPlayers[playerId] = new PlayerData
        {
            PlayerId = playerId,
            Color = playerColor,
            IsJoined = true,
            LeaderBoardEntry = new PlayerLeaderBoardEntry
            {
                Kills = 0,
                Deaths = 0
            }
        };
    }
    
    public bool IsPlayerJoined(int playerId)
    {
        return JoinedPlayers.ContainsKey(playerId);
    }
    
    public int GetPlayerCount()
    {
        return JoinedPlayers.Count;
    }

    public PlayerData GetTopPlayer()
    {
        PlayerData topPlayer = this.JoinedPlayers.Values
            .OrderByDescending(p => p.LeaderBoardEntry.Kills)
            .ThenBy(p => p.LeaderBoardEntry.Deaths)
            .First();
        
        return topPlayer;
    }

    public void Killing(Player killed, Player killer)
    {
        if (_lastKiller == null)
        {
            // Play firstKill
            AddAndPlayAudio(_firstBloodSounds);
            _killingSpreeCount = 1;
        } else if (_lastKiller.PlayerId == killer.PlayerId)
        {
            _killingSpreeCount ++;
            if (_killingSpreeCount == 2)
            {
                // Play doubleKill
                AddAndPlayAudio(_doubleDeathSounds);
            } else if (_killingSpreeCount == 5)
            {
                // Meurtre de masse
                AddAndPlayAudio(_killingSpreeSounds);
            }
        } else
        {
            _killingSpreeCount = 1;
        }
        _lastKiller = JoinedPlayers[killer.PlayerId];
        JoinedPlayers[killer.PlayerId].LeaderBoardEntry.Kills++;
        JoinedPlayers[killed.PlayerId].LeaderBoardEntry.Deaths++;
    }
    
    private void AddAndPlayAudio(AudioStream[] audios)
    {
        var sound = audios[Random.Shared.Next(audios.Length)];
        _killingSpreePlayer.Stream = sound;
        _killingSpreePlayer.VolumeDb = 24;
        _killingSpreePlayer.Play();
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