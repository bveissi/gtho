using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadManager : MonoBehaviour
{
    public float clickDistance;
    public string secretCode;
    public KeypadDisplay display;
    public GameObject correctCodeAction;
    public Camera myCamera;

    public int maxDisplayDigits;

    public AudioSource audioSourceErrorbuzz; // Thease two contains the the audiosources for both sounds made by the keypad - HL
    public AudioSource audioSourceBeep;

    private void Start()
    {
        maxDisplayDigits = secretCode.Length;
        if (myCamera == null)
        {
            myCamera = Camera.main;
        }
    }

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
            var button = hit.collider.gameObject.GetComponent<KeypadButton>();
            if (button != null)
            {
                var name = button.name;
                var text = name.Substring(name.Length - 1);
                ButtonPressed(text);
            }
        }
    }

    public void ButtonPressed(string text)
    {
        if (text == "C")
        {
            display.ClearDisplay();
            audioSourceBeep.PlayOneShot(audioSourceBeep.clip); // Makes keypad beep sound - HL
            return;
        }
        var displayText = display.GetDisplayText();
        if (text == "#") // Enter
        {
            if (displayText == secretCode)
            {
                SecretCodeisOK();
            }
            else
            {
                SecretCodeisNotOK();
            }
        }
        else if (displayText.Length < maxDisplayDigits)
        {
            audioSourceBeep.PlayOneShot(audioSourceBeep.clip); // Makes keypad beep sound - HL

            display.AddDigit(text);
        }
    }

    private void SecretCodeisOK()
    {
        correctCodeAction.SetActive(true); // Activate correct code script, animation and what ever.
    }

    private void SecretCodeisNotOK()
    {
        audioSourceErrorbuzz.PlayOneShot(audioSourceErrorbuzz.clip);
    }
}
