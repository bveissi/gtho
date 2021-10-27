using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeypadDisplay : MonoBehaviour
{
    public TextMesh display;

    private void Start()
    {
        display.text = "";
    }

    public string GetDisplayText()
    {
        return display.text;
    }

    public void ClearDisplay()
    {
        display.text = "";
    }

    public void AddDigit(string digit)
    {
        display.text += digit;
    }

    public void RemovelastDigit()
    {
        var text = display.text;
        if (text.Length > 0)
        {
            if (text.Length == 1)
            {
                ClearDisplay();
            }
            else
            {
                display.text = text.Substring(0, text.Length - 1);
            }
        }
    }
}
