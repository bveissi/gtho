using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{


    public Camera myCamera;
    public float clickDistance;
    public Animator _anim;
    public AudioSource AS;

    public float time;
    public float timer; //This simple timer script counts that there has been atleast one second before you can interact with the drawer again, to prevent spamming the animation, and possibly glitching it


    private void Start()
    {
        time = 1f;
        timer = time;

        if (myCamera == null)
        {
            myCamera = Camera.main;
        }
 
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            handleMouseButtonClick(Input.mousePosition);
        }

    }

    private void handleMouseButtonClick(Vector3 mousePos)
    {
        Ray ray = myCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, clickDistance))
        {

            if (hit.collider.gameObject == gameObject && timer <= 0 && CompareTag("DrawerRR"))
            {
                _anim.SetTrigger("DrawerActionRR");
                timer = time;
                //AS.PlayOneShot(AS.clip); - Here you play a sound, once we have one, remeber to un comment the Audiosource too
            }

            if (hit.collider.gameObject == gameObject && timer <= 0)
            {
                //AS.PlayOneShot(AS.clip); - Here you play a sound, once we have one, remeber to un comment the Audiosource too
                _anim.SetTrigger("DrawerAction");
                timer = time;
            }
        }
    }

    private void DrawerSound()
    {
        AS.pitch = Random.Range(0.6f, 1.1f);
        AS.PlayOneShot(AS.clip);
    }
}
