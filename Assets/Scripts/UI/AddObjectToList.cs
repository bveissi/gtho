using TMPro;
using UnityEngine;

public class AddObjectToList : MonoBehaviour
{
    public LeaderboardUiListItem itemTemplate;
    public GameObject content;

    private void Start()
    {
        var manager = SQLiteManager.Instance;
        var result = manager.QueryTable(10);
        int rank = 0;
        foreach (var leaderboardRow in result) // näytä kaikki saadut LeaderboardRow rivit lokilla
        {
            leaderboardRow.rank = ++rank;
            Debug.Log($"{rank} {leaderboardRow}");
            var listItem = Instantiate<LeaderboardUiListItem>(itemTemplate, content.transform);
            listItem.SetContent(leaderboardRow);
            listItem.gameObject.SetActive(true);
        }
    }

    public void AddButton_Click()
    {
        var listItem = Instantiate<LeaderboardUiListItem>(itemTemplate, content.transform);
        listItem.gameObject.SetActive(true);
        listItem.rank.text = "1";
        listItem.playerName.text = "2";
        listItem.playTime.text = "3";
    }

    public void ShowLeaderboard_Click()
    {
        var manager = SQLiteManager.Instance;
        var result = manager.QueryTable(10);
        int rank = 0;
        foreach (var leaderboardRow in result) // näytä kaikki saadut LeaderboardRow rivit lokilla
        {
            leaderboardRow.rank = ++rank;
            Debug.Log($"{rank} {leaderboardRow}");
            var listItem = Instantiate<LeaderboardUiListItem>(itemTemplate, content.transform);
            listItem.SetContent(leaderboardRow);
            listItem.gameObject.SetActive(true);
        }
    }
}
