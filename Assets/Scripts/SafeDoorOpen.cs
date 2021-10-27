using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeDoorOpen : MonoBehaviour
{
    [SerializeField] private GameObject openObject;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed;
    [SerializeField] private float endRotationY;
    [SerializeField] private Vector3 rotation;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        openObject.transform.Rotate(direction, Time.deltaTime * speed, Space.World);
        rotation = openObject.transform.rotation.eulerAngles;
        if (rotation.y < endRotationY)
        {
            this.enabled = false;
        }
    }
}
