using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class used for the power up pickups
public class PowerUp : MonoBehaviour
{
    //define references
    GameObject player;
    PlayerMovement playerMovement;
    public bool jumpPowerUp;
    public bool runPowerUp;
    bool activated = false;
    public float jump;
    int time = 0;

    float currentPos;
    bool up = true;
    Vector3 moveUp = new Vector3(0, 0.0009f, 0);
    Vector3 moveDown = new Vector3(0, -0.0009f, 0);

    GameObject soundEffect;
    AudioSource eat;

    // Start is called before the first frame update
    void Start()
    {
        //set references
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        currentPos = transform.position.y;
        soundEffect = GameObject.Find("PowerUpSound");
        eat = soundEffect.GetComponent<AudioSource>();
        InvokeRepeating("Timer", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //move power ups up and down
        if (up)
        {
            //move up if there is space to move up
            transform.position += moveUp;

            //if reached the mac height stop moving up
            if (transform.position.y >= currentPos + 0.1f)
            {
                up = false;
            }
        }
        else
        {
            //move down if there is space to move down
            transform.position += moveDown;

            //if reached the min height start moving up again
            if (transform.position.y <= currentPos)
            {
                up = true;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //if player collides with powerup
        if (other.gameObject == player)
        {
            eat.Play();

            //activate powerup on player and remove
            activated = true;
            gameObject.SetActive(false);

            //if it is jump power up then change players jump
            if (jumpPowerUp)
            {
                playerMovement.jumpForce = jump;
            }

            //if it is run power up then change players speed
            if (runPowerUp)
            {
                playerMovement.speed = 9;
            }
        }
    }

    void Timer()
    {
        if (activated)
        {
            time += 1;
        }

        if(time > 5)
        {
            activated = false;

            //change jump back to normal
            if (jumpPowerUp) { 
                playerMovement.jumpForce = 5f;
                time = 0;
            }

            //change sprint back to normal
            if (runPowerUp) { 
                playerMovement.speed = 6f;
                time = 0;
            }
        }
    }
}
