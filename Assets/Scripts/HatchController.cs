using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchController : MonoBehaviour
{

    public Animator radioAnim;

    public bool batteryInserted;

    public AudioSource lidCloseSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            radioAnim.SetTrigger("CloseHatch");
            Destroy(other.gameObject);
            batteryInserted = true;
        }
    }

    public void playLidSound()
    {
        lidCloseSound.PlayOneShot(lidCloseSound.clip);
    }
}
