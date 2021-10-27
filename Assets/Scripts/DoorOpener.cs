using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{

    public Animator _anim;



    // Start is called before the first frame update
    void Start()
    {
        _anim.SetBool("KeyToHole", true);
    }


}
