using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class used for obstacle assests
public class Obstacle : MonoBehaviour
{
    //define references
    GameObject playerObject;
    Rigidbody player;
    PlayerMovement playerMovement;
    GameObject soundEffect;
    AudioSource oofSound;

    // Start is called before the first frame update
    void Start()
    {
        //set references
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Rigidbody>();
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        soundEffect = GameObject.Find("HitSound");
        oofSound = soundEffect.GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider other)
    {
        //if player collides with obstacle and are not already knocked down from another obstacle
        if (other.gameObject == playerObject && !playerMovement.knockedDown)
        {
            //stop player movement and set as knocked down
            playerMovement.runInput = false;
            playerMovement.knockedDown = true;
            oofSound.Play();

            //rotate player to lay on the ground and add force to bounce off obstacle 
            player.transform.Rotate(-80, 0, 0);
            player.AddForce(Vector3.back * 7, ForceMode.Impulse);
        }
    }
}
