using UnityEngine;

/// <summary>
/// Updates mouse cursor state.
/// </summary>
/// <remarks>
/// Global cursor state management is done elsewhere, we just provide means to toggle it on/off on request.
/// </remarks>
public class DebugMouseController : MonoBehaviour
{
    // https://docs.unity3d.com/Manual/PlatformDependentCompilation.html    
#if UNITY_EDITOR
    private void Update()
    {
        // Poll mouse cursor state.
        if (Input.GetMouseButtonDown(1))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
#endif
}
