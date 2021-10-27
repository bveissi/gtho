using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public TMP_Text playerName;
    public Animator transition;

    private void Start()
    {
        if (playerName != null)
        {
            this.playerName.text = HighScoreManager.GetPlayerName();
        }
    }

    public void PlayGame()
    {
        Debug.Log("Start Game!");
        //transition.SetTrigger("Start");
        SceneManager.LoadScene(2);
    }

    public void GoToOptionsMenu()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("CreditsMenu");
    }

    public void GoToControls()
    {
        SceneManager.LoadScene("ControlsMenu");
    }
    public void GoToHighScore()
    {
        SceneManager.LoadScene("HighScore");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
