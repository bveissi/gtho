using UnityEngine;

public class MouseZoom : MonoBehaviour
{
    // https://docs.unity3d.com/ScriptReference/Camera-fieldOfView.html

    public float minFieldOfView;
    public float maxFieldOfView;
    public float zoomSpeed = 0.1f;

    private Camera _camera;
    private MouseLook mouseLook;
    float m_FieldOfView; //This is the field of view that the Camera has

    void Start()
    {
        _camera = Camera.main;
        mouseLook = GetComponent<MouseLook>();
        //takes Main cameras valuat at start
        m_FieldOfView = _camera.fieldOfView;

    }

    void Update()
    {
        var scrollAmount = Input.GetAxis("Mouse ScrollWheel"); // Mouse scroll is -0.1, 0.0 or +0.1 (in my experience)
        if (scrollAmount == 0f)
        {
            return;
        }
        // add adjusted value to current camera field of view.
        m_FieldOfView = _camera.fieldOfView + scrollAmount * mouseLook.Sensitivity * zoomSpeed;
        if (m_FieldOfView < minFieldOfView || m_FieldOfView > maxFieldOfView)
        {
            return;
        }
        //Update the camera's field of view
        _camera.fieldOfView = m_FieldOfView;
    }
}
