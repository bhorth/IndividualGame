using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for collectibles 
public class Collectible : MonoBehaviour
{
    //define references
    GameObject player;
    bool up = true;
    Vector3 moveUp = new Vector3(0, 0.0009f, 0);
    Vector3 moveDown = new Vector3(0, -0.0009f, 0);
    float currentPos;

    GameObject hudUI;
    HUD score;
    GameObject sound;
    AudioSource effect;

    // Start is called before the first frame update
    void Start()
    {
        //set referneces
        player = GameObject.FindGameObjectWithTag("Player");
        currentPos = transform.position.y;
        hudUI = GameObject.Find("HUD");
        score = hudUI.GetComponent<HUD>();
        sound = GameObject.FindGameObjectWithTag("Sound");
        effect = sound.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0.5f, 0); //rotate collectibles 

        //move collectibles up and down
        if (up)
        {
            //if collectible can move up then move position up
            transform.position += moveUp;

            //if collectible reaches max height it cannot move up anymore
            if(transform.position.y >= currentPos + 0.1f)
            {
                up = false;
            }
        }
        else
        {
            //move down if collectbile can move down
            transform.position += moveDown;

            //if collectable reaches min height then start moving up again
            if(transform.position.y <= currentPos)
            {
                up = true;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //if player collides with collectible update score and remove collectible
        if (other.gameObject == player)
        {
            effect.Play();
            score.score += 1;
            gameObject.SetActive(false);
        }
    }
}
