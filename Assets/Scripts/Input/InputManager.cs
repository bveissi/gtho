using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera myCamera;

    [SerializeField]
    private ClickTarget currentTarget;

    private void Start()
    {
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
        ClickTarget clickTarget = null;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            clickTarget = hit.collider.gameObject.GetComponent<ClickTarget>();
        }
        handleClick(clickTarget);
    }

    private void handleClick(ClickTarget clickTarget)
    {
        if (currentTarget != clickTarget)
        {
            if (currentTarget != null)
            {
                currentTarget.onUnselected(); // TODO: add parameter for new selected gameObject!
            }
            if (clickTarget != null)
            {
                clickTarget.onSelected();
            }
            currentTarget = clickTarget;
        }
        if (currentTarget != null)
        {
            clickTarget.onClick();
        }
    }
}
