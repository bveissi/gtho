using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BombDefuse : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject Defuse;
    public GameObject endGameTrigger;
    public GameObject endGameDoor;
    public SafeDoorOpen openDoor;

    public GameObject activateLamp;
    
    private void Start()
    {
        //Resume();

    }

    private void OnDisable()
    {
        // Levelin lopussa vapauta kursori
        Cursor.lockState = CursorLockMode.None;//kursori vapaa
        Cursor.visible = true;//kursori näkyy
    }

    public void Pause()
    {
        Defuse.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;//kursori vapaa
        Cursor.visible = true;//kursori näkyy
    }

    public void Resume()
    {
        Debug.Log("BOMB defuce resume");
        Defuse.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;//kursori lukittu
        Cursor.visible = false;//kursori piilossa
        endGameTrigger.SetActive(true);
        endGameDoor.SetActive(true);
        openDoor.enabled = true;
        activateLamp.SetActive(true);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("GameOverLose");
    }

    public void Correct()
    {
        Defuse.SetActive(false);
        Debug.Log("correct");
    }

}
