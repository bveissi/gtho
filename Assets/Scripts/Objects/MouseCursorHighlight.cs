using UnityEngine;

public class MouseCursorHighlight : MonoBehaviour
{
    // TODO: kun hiirikursori osuus objecktin p‰‰lle jota voi manipuloida, niin hiirikursori muuttuu
    private Camera myCamera;

    private void Update()
    {
        Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && isHightlightObject(hit))
        {
            // highlight cursor
        }
        else
        {
            // restore normal cursor
        }
    }

    private bool isHightlightObject(RaycastHit hit)
    {
        return false;
    }
}
