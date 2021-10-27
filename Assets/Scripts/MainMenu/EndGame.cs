using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public Timer timer;

    private void Start()
    {
        timer = FindObjectOfType<Timer>();
    }

    void OnTriggerEnter(Collider ChangeScene)
    {
        if (ChangeScene.gameObject.CompareTag("Player"))
        {
            var elapsedSeconds = timer.elapsedSeconds;
            var minutes = (int) (elapsedSeconds / 60);
            var seconds = (int)(elapsedSeconds - minutes * 60); // modulo elapsedSeconds % 60 tekee saman laskun
            HighScoreManager.AddHighScore(minutes, seconds);
            SceneManager.LoadScene("EndGame");
        }
    }
}
