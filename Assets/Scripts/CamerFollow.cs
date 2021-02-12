using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for camera folling the players position
public class CamerFollow : MonoBehaviour
{
    //define references
    public Transform player;
    float smoothing = 5f;
    Vector3 offset;
    public bool follow = true;

    void Start()
    {
        //set offset vector from the camera to the plaer
        offset = transform.position - player.position;
    }


    void FixedUpdate()
    {
        if (follow)
        {
            //update camera postion based on player movement
            Vector3 targetCamPos = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}
