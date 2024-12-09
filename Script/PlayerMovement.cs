using System.Collections;
using System.Collections.Generic;

// using System.Numerics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 12.0f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3.0f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // reset velocity if player is on the ground
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // get player input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // create move vector
        Vector3 move = transform.right * x + transform.forward * z; // right -  red axis, forward - blue axis
        
        // move player
        controller.Move(move * speed * Time.deltaTime);

        // check if player can jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // up
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        // down
        velocity.y += gravity * Time.deltaTime;

        // execute jump
        controller.Move(velocity * Time.deltaTime);

        // check if player is moving
        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;

        }
        else
        {
            isMoving = false;
        }

        lastPosition = gameObject.transform.position;
    }
}
