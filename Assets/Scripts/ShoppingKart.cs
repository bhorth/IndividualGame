using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//class used for the shopping kart movement
public class ShoppingKart : MonoBehaviour
{
    //efine refences
    float distance = 2.0f;
    float currentPos;
    Vector3 moveLeft;
    Vector3 moveRight;
    bool atStart = true;
    bool atEnd = false;
    bool left = true;

    public float speed;
    private void Awake()
    {
        //set references
        currentPos = transform.position.x;
        moveLeft = new Vector3(speed, 0, 0);
        moveRight = new Vector3(-speed, 0, 0);

    }
    void Update()
    {
        //check if kart is at its starting position and ending position
        atStart = transform.position.x <= currentPos;
        atEnd = transform.position.x >= currentPos + distance;

        //if at start then start moving left
        if (atStart)
        {
            left = true;
        }

        //if at end then start moving back
        if (atEnd)
        {
            left = false;
        }

        //move kart left
        if (left)
        {
            transform.position += moveLeft;
        }
        //move kart right
        else
        {
            transform.position += moveRight;
        }
    }
}
