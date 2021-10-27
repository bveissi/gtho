using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    private static int lastInsertId;

    public static string GetPlayerName()
    {
        var playerName = PlayerPrefs.GetString("PlayerName");
        return playerName;
    }

    public static string GetLastHighScoreTime()
    {
        var leaderboard = GetLastLeaderboardResult();
        if (leaderboard != null)
        {
            var seconds = leaderboard.seconds;
            return string.Format("{0}:{1:00}", seconds / 60, seconds % 60); // modulo = jakojäännös. Pitää olla kokonaisluku(Int) jotta toimi. ei toimi muuten esim jos on desimaaleja.
        }
        return "unknown";
    }

    public static void AddHighScore(int minutes, int seconds)
    {
        AddHighScore(GetPlayerName(), minutes, seconds);
    }

    private static void AddHighScore(string playerName, int minutes, int seconds)
    {
        Debug.LogFormat("AddHighScore for {0} time {1}:{2:00}", playerName, minutes, seconds);
        var manager = SQLiteManager.Instance;
        var totalTimeInSeconds = minutes * 60 + seconds;
        lastInsertId = manager.InsertRow(playerName, totalTimeInSeconds);
    }

    private static LeaderboardRow GetLastLeaderboardResult()
    {
        var manager = SQLiteManager.Instance;
        var row = manager.QueryRow(lastInsertId);
        return row;
    }
}
