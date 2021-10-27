using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    [SerializeField] private GameObject destroyObject;

    void Start()
    {
        Destroy(destroyObject);
    }
}
