using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNameInput : MonoBehaviour
{
    public TMP_InputField inputField;

    // Start is called before the first frame update
    private void Start()
    {
        var playerName = PlayerPrefs.GetString("PlayerName", "");
        if (!string.IsNullOrWhiteSpace(playerName))
        {
            inputField.text = playerName;
        }
    }

    public void SavePlayerName()
    {
        var playerName = inputField.text;
        if (!string.IsNullOrWhiteSpace(playerName))
        {
            PlayerPrefs.SetString("PlayerName", playerName);
            SceneManager.LoadScene("MainMenu");
        }
    }
}
