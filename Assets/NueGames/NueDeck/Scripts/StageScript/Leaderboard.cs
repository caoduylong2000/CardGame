using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NueGames.NueDeck.Scripts.Managers;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    // Create a leaderboard with this ID in the Unity Cloud Dashboard
    const string WeeklyLeaderboardId = "Weekly_HSG_Leaderboard";

    private string playerId;

    string VersionId { get; set; }
    int Offset { get; set; }
    int Limit { get; set; }
    int RangeLimit { get; set; }
    List<string> FriendIds { get; set; }

    async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    async Task SignInAnonymously()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    
    public async void AddScore()
    {
        await SignInAnonymously();

        var options = new AddPlayerScoreOptions
        {
            Metadata = new Dictionary<string, string>() {
            { "PlayerName", GameManager.Instance.playerName },
            { "CurrentStage", (GameManager.Instance.PersistentGameplayData.CurrentStageId + 1).ToString() },
            { "Gold", GameManager.Instance.PersistentGameplayData.CurrentGold.ToString() },
            { "Time Update", System.DateTime.Now.ToString() }
        }
        };
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(WeeklyLeaderboardId, GameManager.Instance.gameScore, options);
        
    }


    public async Task<string> GetScores()
    {
        await SignInAnonymously();

        var options = new GetScoresOptions { IncludeMetadata = true };
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(WeeklyLeaderboardId, options);
        string jsonString = JsonConvert.SerializeObject(scoresResponse);

        return jsonString;
    }

    public async void GetPaginatedScores()
    {
        Offset = 100;
        Limit = 20;
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(WeeklyLeaderboardId, new GetScoresOptions { Offset = Offset, Limit = Limit });
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPlayerScore()
    {
        var scoreResponse =
            await LeaderboardsService.Instance.GetPlayerScoreAsync(WeeklyLeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async void GetVersionScores()
    {
        var versionScoresResponse =
            await LeaderboardsService.Instance.GetVersionScoresAsync(WeeklyLeaderboardId, VersionId);
        Debug.Log(JsonConvert.SerializeObject(versionScoresResponse));
    }
}
