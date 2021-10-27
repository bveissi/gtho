using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed;
    public float gravity = -20f;
    //public float jumpHeight = 1f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    public bool isWalking;
    public float FootstepDelayTime;
    public AudioSource footstepAS;

    private float nextFootStep = 0f;



    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        if (Input.GetKey("up") || Input.GetKey("down") || Input.GetKey("left") || Input.GetKey("right") || Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d"))
        {
            isWalking = true; // checks if any direction button is pressed, if so, toggles 'isWalking' for the footsteps sound effect to play
            nextFootStep -= Time.deltaTime;

        }
        else
        {
            isWalking = false; // when buttons are not pressed, pålayer is not walking, and the footstep sound stops playing
        }

        if (isWalking == true && nextFootStep <= 0)
        {
            footstepAS.pitch = Random.Range(0.5f, 0.8f);
            footstepAS.volume = Random.Range(0.15f, 0.2f);
            footstepAS.PlayOneShot(footstepAS.clip);
            nextFootStep += FootstepDelayTime;
        }


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        /* Kommentissa, koska en tiedä tuleeko hyppyä?

         if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        */

    }


}
