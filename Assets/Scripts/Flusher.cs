using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flusher : MonoBehaviour
{


    public Camera myCamera;
    public float clickDistance;
    public Animator _anim;
    public AudioSource AS;

    public float time;
    public float timer;


    private void Start()
    {
        time = 3f;
        timer = time;

        if (myCamera == null)
        {
            myCamera = Camera.main;
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            handleMouseButtonClick(Input.mousePosition);
        }
    }

    private void handleMouseButtonClick(Vector3 mousePos)
    {
        Ray ray = myCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, clickDistance))
        {
            if (hit.collider.gameObject == gameObject && timer <= 0)
            {
                AS.PlayOneShot(AS.clip);
                _anim.SetTrigger("flush");
                timer = time;
            }
        }
    }
}
