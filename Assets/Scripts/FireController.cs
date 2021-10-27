using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{

    [SerializeField] private GameObject LightUpAction; // When this is activated, the fire lights up in the fireplace (this is a particle system of the fire itself)
    [SerializeField] private GameObject CracklingSound; // This sound also gets activated when fire is lit, and plays on loop
    [SerializeField] private GameObject BleachWoosh;
    [SerializeField] private GameObject BoxWoosh; 

    public AudioSource bleachWoosh;  // Woosh sound effect for bleach fire
    public AudioSource boxWoosh; // More quiet woosh sound effect for box burn effect (same sound is used, the boxWoosh is just alot more quiet)

    public GameObject cutters;

    public GameObject boxDestination; // a empty gameobject that helps the box to stay inside the fireplace when its placed on the trigger (it wanted to roll out of the fire without this)
    private Vector3 destroyableBoxDestination; // a empty gameobject that helps the destroyable box to stay inside the fireplace when its place on the trigger
    public GameObject burnableBox; // The box "with batteries" itself
    public GameObject destroyableBox; // the prefab that gets instantiated, and later exploded

    public float explosionForce; // Force of the explosion that replaces the coordinations of the destroyable boxes sides
    public float explosionArea; // Area which the explosion impact affects (both of thease are set in inspector for easier acces)

    public PickUp pickUp;
    private bool isBurnableBoxTracked; // Track realBox/burnableBox untill it is destroyed-BV
    private GameObject fracturedBoxObj;

    private void Update() // BV
    {
        if (isBurnableBoxTracked)
        {
            destroyableBoxDestination = burnableBox.transform.position; // Gets the last know position of the burning box, for the instantiation of the box parts at IEnumerator
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            Debug.Log("Fire in the hole!");
            LightUpAction.SetActive(true); // Activates fire
            CracklingSound.SetActive(true); // Activate burning wood sound effect
            Destroy(other.gameObject, 0.2f); // Destroys the object tagged as "Fire", a candle and such, after the given amount of time after colliding
        }

        if (other.CompareTag("Bleach") && LightUpAction.activeInHierarchy == true) // if fire is lit, and bleach is dropped into it, activates woosh sound and visual effect
        {
            Debug.Log("WOOOSH");
            BleachWoosh.SetActive(true); // Activates woosh effect
            bleachWoosh.PlayOneShot(bleachWoosh.clip);
            Destroy(other.gameObject, 0.2f);
        }


        if (other.CompareTag("BurnableBox") && LightUpAction.activeInHierarchy == true) // if fire is lit, and box is dropped into it, activates woosh sound and visual effect
        {
            // Changed burnableBox to Other to harmonize code for better consistency.
            Debug.Log("Box is burning");
            burnableBox.GetComponent<PickUp>().enabled = false; // did not work because OnMouseDown is sent to disabled components :-( - BV
            BoxWoosh.SetActive(true); // Activates woosh effect
            boxWoosh.PlayOneShot(boxWoosh.clip);
            other.gameObject.transform.position = boxDestination.transform.position; // This makes sure that the box won't roll out of fire place while the coroutine is running.
            destroyableBoxDestination = other.gameObject.transform.position; // Gets the last know position of the burning box, for the instantiation of the box parts at IEnumerator
            StartCoroutine(BurnBox()); // Lets the box burn for the amount for time set in the IEnumerators yield (5 seconds at the moment)
            StartCoroutine(RealBox(other.gameObject));
        }
    }

    IEnumerator RealBox(GameObject realBox) // enable box tracking in Update() until it is destoyed- BV
    {
        isBurnableBoxTracked = true;
        yield return new WaitForSeconds(4.8f); // waits for the box to be visible until it is destroyed -BV
        isBurnableBoxTracked = false;
        Destroy(realBox.gameObject);
    }

    IEnumerator BurnBox() 
    {

        yield return new WaitForSeconds(5f); // waits for the box to burn for 5 seconds

        fracturedBoxObj = Instantiate(destroyableBox, destroyableBoxDestination, Quaternion.identity); // Instantiates a prefab of the box, with detached sides as their own objects, at the last know position of the burning box
        
        Rigidbody[] allRigidBodies = fracturedBoxObj.GetComponentsInChildren<Rigidbody>(); // This makes a list of the child objects of the instantiated object (in this case, the detached sides of the new box
        if(allRigidBodies.Length > 0) // if there are rigidbodies in the list (all the detached sides have their own box collider and rigid bodies) do the stuff underneath
        {
            foreach(var body in allRigidBodies)
            {
                body.AddExplosionForce(explosionForce, transform.position, explosionArea); // adds slight amount of explosive force to the children of the destroyable box. This makes the sides pop off eachother
                Destroy(fracturedBoxObj, 3f);
            }
        }

        cutters.SetActive(true);
        cutters.GetComponent<Rigidbody>().AddForce(new Vector3(-1f, 1f, 1f) * 30); //Activates 'Battery' and shoots it out of the fire place
        // this is not needed  because we exit anyway - BV
        //StopCoroutine(BurnBox(destroyableBoxDestination)); // Stops the coroutine after all this is done, so it won't mess with other coroutines, and possibly slow down the game
    }

}
