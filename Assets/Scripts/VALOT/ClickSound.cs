using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSound : MonoBehaviour
{
    public AudioSource LockAS;
    public AudioSource DoorAS;

    public void DoorClick()
    {
        LockAS.PlayOneShot(LockAS.clip);
    }

    public void CreackSound()
    {
        DoorAS.PlayOneShot(DoorAS.clip);
    }
}
