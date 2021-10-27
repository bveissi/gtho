using TMPro;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    public TextMeshProUGUI gameOverLabel;
    public TextMeshProUGUI playerNameLabel;

    // Start is called before the first frame update
    void Start()
    {
        if (gameOverLabel != null)
        {
            gameOverLabel.text = $"Game Over, your time was {HighScoreManager.GetLastHighScoreTime()}";
        }
        playerNameLabel.text = HighScoreManager.GetPlayerName();
    }
}
