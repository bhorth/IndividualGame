using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for the truck movement
public class TruckMovement : MonoBehaviour
{
    //define references
    public GameObject trucks;
    public GameObject player;
    bool move = false;

    float currentPos;
    Vector3 displacement = new Vector3(0, 0, -0.03f);

    private void Start()
    {
        //set reference
        currentPos = trucks.transform.position.z;
    }

    public void OnTriggerEnter(Collider other)
    {
        //if player collides with object trigger start moving the trucks
        if (other.gameObject == player)
        {
            move = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //move the position of the trucks
        if (move)
        {
            trucks.transform.position += displacement;
        }

        //if trucks position reaches certain distance then stop moving them
        if(trucks.transform.position.z <= currentPos - 60)
        {
            move = false;
        }
    }
}
