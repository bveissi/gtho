using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPoint : MonoBehaviour
{

    public float forceFactor; // Teho jolla esine tulee kohti magneettia, s‰‰det‰‰n inspectorissa selkeyden vuoksi. (Mit‰ painavampi esine, sit‰ enemm‰n forcea tarvitaan (mit‰ suurempi RB -> mass))

    List<Rigidbody> rgbKeys = new List<Rigidbody>(); // Jos avaimia olisi kerralla magneetin triggeriss‰ enemm‰nkin, kaikki m‰‰r‰tyll‰ t‰gill‰ olevat esineet lent‰isiv‰t kohti magneettia

    Transform magnetPoint;

    // Start is called before the first frame update
    void Start()
    {
        magnetPoint = GetComponent<Transform>(); // Tekee triggerin sijainnista pisteen jota kohti esineet tulevat
    }

    private void FixedUpdate()
    {
        foreach(Rigidbody rgbKey in rgbKeys) // Jokainen listassa oleva rigidbody saa voimaa miinus asteikolla AddForcesta, ja lent‰‰ 'force factorin' mukaisen tehon voimalla takaperin, eli t‰ss‰ tilanteessa kohti magneettia
        {
            rgbKey.AddForce((magnetPoint.position - rgbKey.position) * forceFactor * Time.fixedDeltaTime);

            // Alla oleva velocity ja angularVelocity lis‰tty itse jotta puoleen vedett‰v‰ esine tulisi pelaajalle tasaisesti ja hitaasti, mutta siihen voisi kohdistaa tarkoituksella enemm‰n voimaa
            // (kuten liu'uttaakseen esineen alas hyllylt‰).
            // En ole ihan varma miksi t‰m‰ toimii niinkuin se toimii, mutta tuloksena on miellytt‰v‰ magneetti.
            // Luulen ett‰ koska updatessa pakotetaan velocity‰ fixedframella nollaan ja addforce haluaa lis‰t‰ sit‰ kokoajan, tapahtuu t‰m‰ hidastuminen. - HL

            rgbKey.velocity = new Vector3(0, 0, 0);
            rgbKey.angularVelocity = new Vector3(0, 0, 0);
            
        }
    }

    private void OnTriggerEnter(Collider other) // Ottaa haltuun t‰gin mukaisen objektin rigidbodyn lis‰‰m‰ll‰ sen tilap‰isesti listaan 'rgbKeys', joka sittemmin tottelee t‰m‰n scriptin FixedUpdatea
    {
        if (other.CompareTag("LivingroomKey"))
        {
            rgbKeys.Add(other.GetComponent<Rigidbody>());
            Debug.Log("Dragging");
        }
    }

    private void OnTriggerExit(Collider other) // Lakkaa ohjaamasta objektin Rigidbody‰ kun se ei en‰‰ ole triggerin vaikutuksen piiriss‰, eik‰ t‰ten en‰‰ listalla
    {
        if (other.CompareTag("LivingroomKey"))
        {
            rgbKeys.Remove(other.GetComponent<Rigidbody>());
            Debug.Log("Dragging stopped");
        }

    }
}
