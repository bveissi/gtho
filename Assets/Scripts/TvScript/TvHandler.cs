using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvHandler : MonoBehaviour
{
    [SerializeField]
    private Camera myCamera;
    [SerializeField]
    private float clickDistance;

    [SerializeField]
    private GameObject turnOn;
    [SerializeField]
    private GameObject turnOff;
    [SerializeField]
    private GameObject turnChannel;
    [SerializeField]
    private GameObject changeVolume;

    [SerializeField]
    private GameObject[] channels;
    [SerializeField]
    private GameObject[] channelEffects;

    [SerializeField]
    private AudioSource clickSound;
    [SerializeField]
    private AudioSource tvStaticNoise;
    [SerializeField]
    private AudioSource tvNoSignal;

    [SerializeField]
    private Animator channelKnob;

    [Header("Live Data")]
    [SerializeField]
    private bool isTvOn;
    [SerializeField]
    private int channelIndex;


    private void Start()
    {
        if (myCamera == null)
        {
            myCamera = Camera.main;
        }
        for (var i = 0; i < channels.Length; ++i)
        {
            setChannel(i, false);
        }
        isTvOn = true;
        handleTvOff();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
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
            var clickTarget = hit.collider.gameObject;
            switch (clickTarget.tag)
            {
                case "TvOn":
                    handleTvOn();
                    clickSound.pitch = Random.Range(1.2f, 1.8f);
                    clickSound.PlayOneShot(clickSound.clip); // Thease one shots are the clicking sound when you interact with television
                    break;
                case "TvOff":
                    handleTvOff();
                    clickSound.pitch = Random.Range(1.2f, 1.8f);
                    clickSound.PlayOneShot(clickSound.clip);
                    break;
                case "TvChannel":
                    handleTvChannel();
                    channelKnob.SetTrigger("chAction");
                    clickSound.pitch = Random.Range(1.2f, 1.8f);
                    clickSound.PlayOneShot(clickSound.clip);
                    break;
                case "TvVolume":
                    handleTvVolume();
                    break;
            }
        }
    }

    private void handleTvOn()
    {
        if (!isTvOn)
        {
            isTvOn = true;
            setChannel(0, false); // Turn "off" channel off
            channelIndex = 1; // First TV on channel
            setChannel(channelIndex, true); // Turn  it on
        }
    }
    private void handleTvOff()
    {
        if (isTvOn)
        {
            isTvOn = false;
            setChannel(channelIndex, false); // Turn current channel off
            channelIndex = 0; // TV "off" channel
            setChannel(channelIndex, true); // Turn "off" channel on
        }
    }
    private void handleTvChannel()
    {
        if (isTvOn)
        {
            setChannel(channelIndex, false); // Turn current channel off
            channelIndex += 1;
            if (channelIndex >= channels.Length)
            {
                channelIndex = 1; // Skip cahnnel 0 which is TV off "channel"
            }
            setChannel(channelIndex, true); // Turn new current channel on
        }
    }
    private void handleTvVolume()
    {
        if (isTvOn)
        {
            Debug.Log("volume");
        }
    }

    private void setChannel(int index, bool state)
    {
        channels[index].SetActive(state);
        if (channelEffects[index] != null)
        {
            channelEffects[index].SetActive(state);
        }
    }
}
