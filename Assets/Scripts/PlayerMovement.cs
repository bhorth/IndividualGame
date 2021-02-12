using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for the players movment
public class PlayerMovement : MonoBehaviour
{
    //define references
    public Animator animator;
    public Rigidbody player;

    public bool runInput = false;
    public bool knockedDown = false;

    Vector3 movement;
    public float speed = 6f;
    private float jumpTime = 0;
    private float jumpInterval = 0.25f;
    private bool jumpInput = false;
    public float jumpForce = 5f;

    private List<Collider> collisions = new List<Collider>();

    private bool isGrounded;
    private bool wasGrounded;

    GameObject hudUI;
    HUD timer;
    public bool finished = false;

    private void Start()
    {
        //set references
        hudUI = GameObject.Find("HUD");
        timer = hudUI.GetComponent<HUD>();
    }


    private void Update()
    {
        //if player isnt jumping already and player inputs jump key
        if (!jumpInput && Input.GetKey(KeyCode.Space))
        {
            //if the player has been knocked down by obstacle rotate player back up, set run to true and knocked down false
            if (knockedDown)
            {
                player.transform.Rotate(80, 0, 0);
                knockedDown = false;
                runInput = true;
            }
            //set jump input as true
            else
            {
                jumpInput = true;
            }
            
        }

        //if user inputs run key and hasnt finished the game set run input as true
        if (Input.GetKey(KeyCode.W) && !finished)
        {
            runInput = true;

            //start timer first time player starts moving
            if (!timer.startTimer)
            {
                timer.startTimer = true;
            }

            //if knocked down rotate player back up
            if (knockedDown)
            {
                player.transform.Rotate(80, 0, 0);
                knockedDown = false;
            }
        }

        //if player reached finish then stop run inputs
        if(runInput && finished)
        {
            runInput = false;
        }
    }
    private void FixedUpdate()
    {
        //set player animation if player is on the ground
        animator.SetBool("Grounded", isGrounded);

        //get horizontal input
        float h = Input.GetAxisRaw("Horizontal");
        float v = 1.0f; //default value for vertical input

        //if theres run input then move the player
        if (runInput)
        {
            Move(h, v);
        }
        //if no movement then stop the running animation
        else
        {
            animator.SetFloat("MoveSpeed", 0f);
        }


        Jump();

        wasGrounded = isGrounded;
        jumpInput = false;
    }

    //check for player collisons on surface to see if player is grounded
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!collisions.Contains(collision.collider))
                {
                    collisions.Add(collision.collider);
                }
                isGrounded = true;
            }
        }
    }

    //check if player is still touching the surface
    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            isGrounded = true;
            if (!collisions.Contains(collision.collider))
            {
                collisions.Add(collision.collider);
            }
        }
        else
        {
            if (collisions.Contains(collision.collider))
            {
                collisions.Remove(collision.collider);
            }
            if (collisions.Count == 0) { isGrounded = false; }
        }
    }

    //check if player exited surface
    private void OnCollisionExit(Collision collision)
    {
        if (collisions.Contains(collision.collider))
        {
            collisions.Remove(collision.collider);
        }
        if (collisions.Count == 0) { isGrounded = false; }
    }

    //fn for moving the player
    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        player.MovePosition(transform.position + movement);
        animator.SetFloat("MoveSpeed", 10.0f);
    }

    //fn for player jumping
    private void Jump()
    {
        //check if the jump cool down is done
        bool jumpCooldownOver = (Time.time - jumpTime) >= jumpInterval;

        //if no jump cool down and player is on ground and there jump input then make the player jump
        if (jumpCooldownOver && isGrounded && jumpInput)
        {
            jumpTime = Time.time;
            player.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        //set the land animation
        if (!wasGrounded && isGrounded)
        {
            animator.SetTrigger("Land");
        }

        //set the jump animation
        if (!isGrounded && wasGrounded)
        {
            animator.SetTrigger("Jump");
        }
    }
}
