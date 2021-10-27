using TMPro;
using UnityEngine;

public class LeaderboardUiListItem : MonoBehaviour
{
    public TextMeshProUGUI rank;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playTime;

    public void SetContent(LeaderboardRow row)
    {
        rank.text = row.rank.ToString();
        playerName.text = row.name;
        playTime.text = row.displayResult;
    }
}
