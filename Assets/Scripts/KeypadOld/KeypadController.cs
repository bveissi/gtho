using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadController : MonoBehaviour, IKeypadController
{
    [SerializeField] private GameObject activation;
    [SerializeField] private GameObject numPad;
    [SerializeField] private GameObject display;
    [SerializeField] private GameObject correctCodeAction;
    [SerializeField] private string secretCode;

    [Header("Live Data")]
    [SerializeField] private bool isActive;
    [SerializeField] private TextMesh displayText;

    private WaitForSeconds deactivationDelay = new WaitForSeconds(0.1f);

    private void Start()
    {
        var children = activation.GetComponentsInChildren<ClickTarget>();
        if (children != null && children.Length == 1)
        {
            children[0].setKeypadController(this);
        }
        children = numPad.GetComponentsInChildren<ClickTarget>();
        if (children != null)
        {
            foreach (var child in children)
            {
                child.setKeypadController(this);
            }
        }
        displayText = display.GetComponentsInChildren<TextMesh>()[0];
        displayText.text = "";
    }

    public void onSelected(GameObject gameObject)
    {
        if (!isActive)
        {
            isActive = true;
            activate();
        }
        StopAllCoroutines();
    }

    public void onClick(GameObject gameObject)
    {
        var text = gameObject.GetComponentsInChildren<TextMesh>()[0].text;
        if (text == "C")
        {
            displayText.text = "";
        }
        else if (text == "#") // Enter
        {
            if (displayText.text == secretCode)
            {
                correctCodeAction.SetActive(true); // Activate correct code script, animation and what ever.
            }
            else
            {
                // TODO: play beep sound
            }
        }
        else
        {
            if (text.Length != 1)
            {
                Debug.LogWarning("keypad config error " + gameObject.name + " has text [" + text + "]");
            }
            else if (displayText.text.Length < secretCode.Length)
            {
                displayText.text += text;
            }
        }
    }

    public void onUnselected(GameObject gameObject)
    {
        StartCoroutine(startDeactivate()); // if pressed outside our componetns we will de-activate poutself after delay
    }

    private IEnumerator startDeactivate()
    {
        yield return deactivationDelay;

        isActive = false;
        deactivate();
    }

    private void activate()
    {
        // TODO: set keypad to visually "active" state
    }

    private void deactivate()
    {
        // TODO: set keypad to visually "in-active" state
    }
}
