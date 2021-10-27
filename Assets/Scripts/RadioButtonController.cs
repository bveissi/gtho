using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioButtonController : MonoBehaviour
{


    public Camera myCamera;
    public float clickDistance;
    public Animator _anim;
    public AudioSource clickSound;
    public AudioSource turnOnStaticClip;
    public AudioSource radioSpeech;

    [Range (0.0f, 1.5f)]
    public float radioSpeechVolume;

    public float time;
    public float timer;

    public bool powerOn;

    public GameObject radioLight;

    public HatchController hCtrl;


    private void Start()
    {
        time = 1f;
        timer = time;

        if (myCamera == null)
        {
            myCamera = Camera.main;
        }

        if(clickSound == null || turnOnStaticClip == null || radioLight == null || hCtrl == null)
        {
            return;
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
            if(hCtrl.batteryInserted == false)
            {
                return;
            }

            if (hit.collider.gameObject == gameObject && timer <= 0 && CompareTag("PowerKnob") && hCtrl.batteryInserted == true)
            {
                clickSound.PlayOneShot(clickSound.clip);
                _anim.SetTrigger("PowerPress");
                timer = time;

                if (!powerOn)
                {
                    powerOn = true;
                    turnOnStaticClip.pitch = Random.Range(0.8f, 1.1f);
                    turnOnStaticClip.PlayOneShot(turnOnStaticClip.clip);
                    radioSpeech.volume = radioSpeechVolume;
                    radioLight.SetActive(true);
                }
                else
                {
                    radioLight.SetActive(false);
                    radioSpeech.volume = 0f;
                    powerOn = false;
                }
            }

            if (hit.collider.gameObject == gameObject && timer <= 0 && CompareTag("TunerKnob"))
            {
                _anim.SetTrigger("TunerTurn");
                timer = time;
            }
        }
    }
}
