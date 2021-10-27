using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLight : MonoBehaviour
{
    public float clickDistance;
    public GameObject[] lights;
    public GameObject[] fakeLights;
    public Camera myCamera;
    private bool lightState = false;

    public GameObject msgCanvas; // msgCanvas in bedroom - HL

    public AudioSource audioSource; // set the click sound audio file in inspector - HL

    public Animator _anim;


    private void Start()
    {
        if (myCamera == null)
        {
            myCamera = Camera.main;
        }

        lightState = lights[0].activeSelf; // Take initial state from  first light
        setLights(lightState,  lights);
        setLights(!lightState, fakeLights);
    }

    /*private void OnTriggerStay(Collider plyr)
    {
        if (plyr.tag == "Player" && Input.GetKeyDown(KeyCode.E) && !on)
        {
            Debug.Log("Valo laitettu p‰‰lle");
            llight.SetActive(true);
            on = true;
        }
        else if (plyr.tag == "Player" && Input.GetKeyDown(KeyCode.E) && on)
        {
            Debug.Log("Valo laitettu kiinni");
            llight.SetActive(false);
            on = false;
        }
    }*/

    private void Update()
    {
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
            if (hit.collider.gameObject == gameObject)
            {
                audioSource.PlayOneShot(audioSource.clip); // Makes click sound - HL
                _anim.SetTrigger("click");
                lightState = !lightState;
                setLights(lightState, lights);
                setLights(!lightState, fakeLights);
                if (lightState == false)
                {
                    if (msgCanvas != null)
                    {
                        msgCanvas.SetActive(true); // Activates the message canvas in the room (only used in bedroom for now) - HL
                    }
                }
                else
                {
                    if (msgCanvas != null)
                    {
                        msgCanvas.SetActive(false); // Deactivates the message canvas in the room (only used in bedroom for now) - HL
                    }
                }
            }
        }
    }

    private void setLights(bool state, GameObject[] lightArray)
    {
        /* jos tarvitse tiet‰‰ taulukon alkion j‰rjestysnumeron eli indeksin, k‰yt‰ t‰t‰
        for (var i = 0; i < lights.Length; ++i)
        {
            lights[i].SetActive(state);
        }*/

        // tarvitset vain kaikki taulukon alkiot j‰rjestyksess‰ alusta loppuun
        foreach (var llight in lightArray)
        {
            llight.SetActive(state);
        }
    }
}
