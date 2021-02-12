using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for the cars movement and collision with player
public class CarMovment : MonoBehaviour
{
    //define references
    Vector3 currentPosition;
    Vector3 move = new Vector3(0.1f, 0, 0);
    public int displacement;
    GameObject player;
    Rigidbody playerBody;
    CamerFollow playerCam;
    GameObject barrier;

    GameObject hudUI;
    HUD hudScript;

    GameObject soundEffect;
    AudioSource hit;

    // Start is called before the first frame update
    void Start()
    {
        //set references
        player = GameObject.FindGameObjectWithTag("Player");
        playerBody = player.GetComponent<Rigidbody>();
        currentPosition = transform.position;
        playerCam = Camera.main.GetComponent<CamerFollow>();
        barrier = GameObject.Find("Barrier");

        hudUI = GameObject.Find("HUD");
        hudScript = hudUI.GetComponent<HUD>();

        soundEffect = GameObject.Find("CarHit");
        hit = soundEffect.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //move the position of the car
        transform.position += move;

        //if car has moved and reached end position set cars postion back to current position
        if (transform.position.x > currentPosition.x + displacement)
        {
            transform.position = currentPosition;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //if player collides with car stop camera following player and add force to player
        //player dies when hit by car
        if (other.gameObject == player)
        {
            hit.Play();
            barrier.SetActive(false);
            playerBody.AddForce(Vector3.right * 200, ForceMode.Impulse);
            playerCam.follow = false;
            hudScript.playerDead = true;
        }
    }
}
