using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomLockController : MonoBehaviour
{

    [SerializeField] private GameObject OpeningAction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BathroomKey"))
        {
            Debug.Log("Avain reijässä");
            OpeningAction.SetActive(true); // Activate correct code script, animation and what ever.
            Destroy(other.gameObject);
        }
    }

}
