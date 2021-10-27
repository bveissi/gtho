using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{

    /// <summary>
    /// Tween koodi. /opiskeltu https://github.com/GabrielCapeletti/PotaTween 
    /// ja  https://stackoverflow.com/questions/27119906/animate-move-translate-tween-image-in-unity-4-6-from-c-sharp-code
    /// </summary>
    public Camera myCamera;
    public float clickDistance;
    [SerializeField] private GameObject openObject;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed;
    [SerializeField] private Vector2 startRotation;
    [SerializeField] private Vector2 endRotation;
    [SerializeField] private float rotationTolerance;
    [SerializeField] private bool testAxisX = true;
    [Header("Live Data")]
    [SerializeField] private Vector3 rotation;
    [SerializeField] private bool startPosition;
    [SerializeField] private bool running;

    [SerializeField] private AudioSource _AS;

    private void Start()
    {
        if (myCamera == null)
        {
            myCamera = Camera.main;
        }
        startRotation = openObject.transform.rotation.eulerAngles;
        startPosition = true;
        running = false;
        rotation = openObject.transform.rotation.eulerAngles;

        if(_AS == null)
        {
            return;
        }
    }

    private void Update()
    {
        if (running)
        {
            move();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            handleMouseButtonClick(Input.mousePosition);
        }
        else
        {
            rotation = openObject.transform.rotation.eulerAngles; // Just for Editor debugging
        }
    }

    private void handleMouseButtonClick(Vector3 mousePos)
    {
        Ray ray = myCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, clickDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                Debug.Log("START");
                running = true;

                if (CompareTag("FreezerDoor"))
                {
                    _AS.pitch = Random.Range(1.2f, 1.3f);
                }
                else if (CompareTag("FridgeDoor"))
                {
                    _AS.pitch = Random.Range(1.1f, 1.2f);
                }
                else
                {
                    _AS.pitch = Random.Range(0.7f, 0.8f);
                }
                _AS.PlayOneShot(_AS.clip);
                
            }
        }
    }

    private void move()
    {
        if (startPosition)
        {
            openObject.transform.Rotate(direction, Time.deltaTime * speed, Space.World);
        }
        else
        {
            openObject.transform.Rotate(-direction, Time.deltaTime * speed, Space.World);
        }
        rotation = openObject.transform.rotation.eulerAngles;
        if (!canRotateMore())
        {
            stop();
        }
    }

    private bool canRotateMore()
    {
        Vector2 rotationTest;
        if (startPosition)
        {
            rotationTest = endRotation;
        }
        else
        {
            rotationTest = startRotation;
        }
        if (testAxisX)
        {
            if (Mathf.Abs(rotation.x - rotationTest.x) < rotationTolerance)
            {
                Debug.Log($"NO MORE X rotation={rotation} rotationTest={rotationTest}");
                return false;
            }
        }
        else
        {
            if (Mathf.Abs(rotation.y - rotationTest.y) < rotationTolerance)
            {
                Debug.Log($"NO MORE Y rotation={rotation} rotationTest={rotationTest}");
                return false;
            }
        }
       /*f (Mathf.Abs(rotation.y - rotationTest.y) < rotationTolerance)
        {
            Debug.Log($"NO MORE Y rotation={rotation} rotationTest={rotationTest}");
            return false;
        }
        if (Mathf.Abs(rotation.x - rotationTest.x) < rotationTolerance)
        {
            Debug.Log($"NO MORE X rotation={rotation} rotationTest={rotationTest}");
            return false;
        }*/
        return true;
    }

    private void stop()
    {
        running = false;
        startPosition = !startPosition; // Reverse direction when stopped.
        Debug.Log("STOPPED");
    }
}
