using UnityEngine;

/// <summary>
/// Interface to handle mouse clicking on 3D objects to select, click and unselect them.
/// </summary>
public interface IKeypadController
{
    /// <summary>
    /// Called when given gameObejct is selected.
    /// </summary>
    void onSelected(GameObject gameObject);

    /// <summary>
    /// Given gameObject has been clicked.
    /// </summary>
    void onClick(GameObject gameObject);

    /// <summary>
    /// Called when given gameObject is unselected.
    /// </summary>
    void onUnselected(GameObject gameObject);
}

/// <summary>
/// Marker class for clickable 3D objects.
/// </summary>
public class ClickTarget : MonoBehaviour
{
    private IKeypadController controller;

    public void setKeypadController(IKeypadController controller)
    {
        this.controller = controller;
    }

    public void onSelected()
    {
        controller.onSelected(gameObject);
    }

    public void onClick()
    {
        controller.onClick(gameObject);
    }

    public void onUnselected()
    {
        controller.onUnselected(gameObject);
    }
}
