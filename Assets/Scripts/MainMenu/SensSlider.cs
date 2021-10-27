using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensSlider : MonoBehaviour
{
    [SerializeField]
    private GameObject _FPSCamera;

    public void SetSens(float mouseSpeed)
    {
        _FPSCamera.GetComponent<MouseLook>().SetMouseSensitivity(mouseSpeed);
    }
}
