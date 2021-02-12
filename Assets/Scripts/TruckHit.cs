using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for truck hitting the player
public class TruckHit : MonoBehaviour
{

    //define references
    GameObject player;
    Rigidbody playerBody;
    CamerFollow playerCam;

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
        playerCam = Camera.main.GetComponent<CamerFollow>();

        hudUI = GameObject.Find("HUD");
        hudScript = hudUI.GetComponent<HUD>();

        soundEffect = GameObject.Find("CarHit");
        hit = soundEffect.GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider other)
    {
        //if player collides with truck then camera stops following, add force back to player, and player dies
        if (other.gameObject == player)
        {
            hit.Play();
            playerBody.AddForce(Vector3.back * 200, ForceMode.Impulse);
            playerCam.follow = false;
            hudScript.playerDead = true;
        }
    }
}
