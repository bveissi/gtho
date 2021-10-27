using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDef : MonoBehaviour
{
    public GameObject canvas;
    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bomb")
        {
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;//kursori vapaa
            Cursor.visible = true;//kursori n�kyy
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bomb")
        {
            canvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;//kursori lukittu
            Cursor.visible = false;//kursori piilossa
        }
    }
}
