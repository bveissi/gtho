using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSmall : MonoBehaviour
{
    private const string destinationName = "Destination2";
    public Transform TheDest; //Näkyy inspekrorissa.

    public bool mouseIsPressed; //Tutkii onko pelaajan hiiri painettuna vai ei - HL

    private float rotaX = 360f; //esineen pyörityksen nopeusarvot - HL
    private float rotaY = 360f;


    public float maxDistanceToPlayer = 3f;  // esineen suurin etäisyys pelaajaan jolla nostaa esineen - BV
    public float distanceToPlayer; // Etäisyys pelaajaan kun klikattu - BV
    public Transform player;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // ettii pelaajan automaattisesti ¨Player¨ tagi - BV
    }

    private void OnMouseDown() //Hiiren painallus
    {
        distanceToPlayer = (this.transform.position - player.position).magnitude;  // Laskee pelaajan etäisyys objectiin - BV
        if (distanceToPlayer < maxDistanceToPlayer) // jos etäisyys on pienempi kuin maksimi etäisyys ehto toteutuu - BV
        {
            GetComponent<Rigidbody>().useGravity = false; // Objektin Gravity pois päältä.
            this.transform.position = TheDest.position; // Siirtää halutun objektin Destination paikalle.
            this.transform.parent = GameObject.Find(destinationName).transform; // Tekee Destinatiosta parentin.
            if (CompareTag("Towel") || CompareTag("BathroomKey"))
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }

            mouseIsPressed = true; //Asettaa boolin trueksi, jotta updatessa olevaa toimintoa voidaan tehdä hiiren ollessa painettuna ja pelaajalla on esine kädessään - HL
        }

    }
    private void OnMouseUp() // Kun vapauttaa hiiren painalluksen
    {
        if (mouseIsPressed)
        {
            this.transform.parent = null; // Objekti ei ole enään Destinationin Child.
            GetComponent<Rigidbody>().useGravity = true; // Objektin Gravity takaisin päälle.

            mouseIsPressed = false; //Asettaa boolin falseksi, jotta updatessa olevaa toimintoa ei voida tehdä hiiren ollessa normaalitilassa ja/tai kun pelaajalla ei ole esinettä hallussaan - HL
        }
    }
    /*
     * Scripti laitetaan haluttuun liikutettavaan objektiin!
     * Pelaajasta löytyy Empty GameObject nimeltä Destination. Tämä siirretään Objektin Inspektorin kohtaan "The Dest".
     * Näin saadaan liikutettava objekti ennalta määriteltyyn paikkaan, joka liikkuu pelaajan mukana.
     * JHe
     */

    private void Update()
    {



        if (mouseIsPressed) // Kun pelaajalla on hiiri sekä painettuna pohjaan, että hallussaan jokin esine jolla on PickUpSmall- scripti, tehdään toimintoa - HL
        {
            // Nämä kaksi riviä estävät kädessä olevien esineiden leijumisen karkuun kun ne osuvat toisiin collidereihin. Ne määräävät että esineellä ei voi olla kiihtyvyytä kun se on pelaajan hallussa. - HL
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);

            if (CompareTag("Fire")) // Jos kädessä oleva esine on tägillä "Painting", sitä ei voi pyörittää koska sille tehtävä update loppuu tähän, ja esineiden pyöritys jatkuu tästä eteenpäin. - HL
                return;

            if (Input.GetKey(KeyCode.Mouse1)) //Pyörittää esinettä x - akselilla positiiviseen suuntaan 360 astetta sekunnissa (nopeus kovakoodattu scriptin alkuun) painamalla hiiren Right-clickiä - HL
            {
                this.transform.Rotate(rotaX * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.Q)) //Pyörittää esinettä y - akselilla positiiviseen suuntaan 360 astetta sekunnissa (nopeus kovakoodattu scriptin alkuun) painamalla 'Q':ta - HL
            {
                this.transform.Rotate(0, rotaY * Time.deltaTime, 0);
            }
            if (Input.GetKey(KeyCode.E)) //Pyörittää esinettä y - akselilla negatiiviseen suuntaan 360 astetta sekunnissa (nopeus kovakoodattu scriptin alkuun) painamalla 'E':ta - HL
            {
                this.transform.Rotate(0, -rotaY * Time.deltaTime, 0);
            }

        }
    }
}
