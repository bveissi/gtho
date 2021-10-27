using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private const string destinationName = "Destination";
    public Transform TheDest; //N‰kyy inspekrorissa.

    public bool mouseIsPressed; //Tutkii onko pelaajan hiiri painettuna vai ei - HL

    private float rotaX = 360f; //esineen pyˆrityksen nopeusarvot - HL
    private float rotaY = 360f;
 

    public float maxDistanceToPlayer = 3f;  // esineen suurin et‰isyys pelaajaan jolla nostaa esineen - BV
    public float distanceToPlayer; // Et‰isyys pelaajaan kun klikattu - BV
    public Transform player; 

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // ettii pelaajan automaattisesti ®Player® tagi - BV
    }

    private void OnEnable()
    {
        //Debug.Log("OnEnable " + name);
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable " + name);
        if (mouseIsPressed) // Jos meid‰t disabloidaan samalla kun kannetaan esinett‰ niin pudota se heti
        {
            this.transform.parent = null; // Objekti ei ole en‰‰n Destinationin Child.
            GetComponent<Rigidbody>().useGravity = true; // Objektin Gravity takaisin p‰‰lle.
                                                         //GetComponent<BoxCollider>().enabled = true; // poistaa objektista stabiilisuuden.

            mouseIsPressed = false; //Asettaa boolin falseksi, jotta updatessa olevaa toimintoa ei voida tehd‰ hiiren ollessa normaalitilassa ja/tai kun pelaajalla ei ole esinett‰ hallussaan - HL
        }
    }

    private void OnMouseDown() //Hiiren painallus
    {
        if (!this.enabled) return; // jos meid‰t on disabloitu niin hiiren painallukset eiv‰t saa vaikuttaa
        distanceToPlayer = (this.transform.position - player.position).magnitude;  // Laskee pelaajan et‰isyys objectiin - BV
        if (distanceToPlayer < maxDistanceToPlayer) // jos et‰isyys on pienempi kuin maksimi et‰isyys ehto toteutuu - BV
        {
            //GetComponent<BoxCollider>().enabled = false; // Tekee objektista stabiilin.
            GetComponent<Rigidbody>().useGravity = false; // Objektin Gravity pois p‰‰lt‰.
            this.transform.position = TheDest.position; // Siirt‰‰ halutun objektin Destination paikalle.
            this.transform.parent = GameObject.Find(destinationName).transform; // Tekee Destinatiosta parentin.

            mouseIsPressed = true; //Asettaa boolin trueksi, jotta updatessa olevaa toimintoa voidaan tehd‰ hiiren ollessa painettuna ja pelaajalla on esine k‰dess‰‰n - HL
        }

    }
    private void OnMouseUp() // Kun vapauttaa hiiren painalluksen
    {
        if (!this.enabled) return; // jos meid‰t on disabloitu niin hiiren painallukset eiv‰t saa vaikuttaa
        if (mouseIsPressed)
        {
            this.transform.parent = null; // Objekti ei ole en‰‰n Destinationin Child.
            GetComponent<Rigidbody>().useGravity = true; // Objektin Gravity takaisin p‰‰lle.
                                                         //GetComponent<BoxCollider>().enabled = true; // poistaa objektista stabiilisuuden.

            mouseIsPressed = false; //Asettaa boolin falseksi, jotta updatessa olevaa toimintoa ei voida tehd‰ hiiren ollessa normaalitilassa ja/tai kun pelaajalla ei ole esinett‰ hallussaan - HL
        }
    }
    /*
     * Scripti laitetaan haluttuun liikutettavaan objektiin!
     * Pelaajasta lˆytyy Empty GameObject nimelt‰ Destination. T‰m‰ siirret‰‰n Objektin Inspektorin kohtaan "The Dest".
     * N‰in saadaan liikutettava objekti ennalta m‰‰riteltyyn paikkaan, joka liikkuu pelaajan mukana.
     * JHe
     */

    private void Update()
    {
        if (mouseIsPressed) // Kun pelaajalla on hiiri sek‰ painettuna pohjaan, ett‰ hallussaan jokin muu esine kuin taulu, ja sill‰ esineell‰ on pickup- scripti, tehd‰‰n toimintoa - HL
        {
            if (!this.enabled) throw new UnityException("should not happen");

            // N‰m‰ kaksi rivi‰ est‰v‰t k‰dess‰ olevien esineiden leijumisen karkuun kun ne osuvat toisiin collidereihin. Ne m‰‰r‰‰v‰t ett‰ esineell‰ ei voi olla kiihtyvyyt‰ kun se on pelaajan hallussa. - HL
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);

            if (CompareTag("Painting")) // Jos k‰dess‰ oleva esine on t‰gill‰ "Painting", sit‰ ei voi pyˆritt‰‰ koska sille teht‰v‰ update loppuu t‰h‰n, ja esineiden pyˆritys jatkuu t‰st‰ eteenp‰in. - HL
                return;

            if (Input.GetKey(KeyCode.Mouse1)) //Pyˆritt‰‰ esinett‰ x - akselilla positiiviseen suuntaan 360 astetta sekunnissa (nopeus kovakoodattu scriptin alkuun) painamalla hiiren Right-clicki‰ - HL
            {
                this.transform.Rotate(rotaX * Time.deltaTime, 0, 0); 
            }
            if (Input.GetKey(KeyCode.Q)) //Pyˆritt‰‰ esinett‰ y - akselilla positiiviseen suuntaan 360 astetta sekunnissa (nopeus kovakoodattu scriptin alkuun) painamalla 'Q':ta - HL
            {
                this.transform.Rotate(0, rotaY * Time.deltaTime, 0);
            }
            if (Input.GetKey(KeyCode.E)) //Pyˆritt‰‰ esinett‰ y - akselilla negatiiviseen suuntaan 360 astetta sekunnissa (nopeus kovakoodattu scriptin alkuun) painamalla 'E':ta - HL
            {
                this.transform.Rotate(0, -rotaY * Time.deltaTime, 0);
            }


        }
    }


}
