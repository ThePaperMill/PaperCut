/****************************************************************************/
/*!
    \author Jerry Nacier
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class SpawnPuppies : MonoBehaviour
{
    //Bool that makes it so that puppies are spawned only once and not every frame
    bool PuppiesAreSpawned = false;

    //Puppy gameobject... oh wait... I don't think we use this var bc a local var is used in the PuppyCannon func
    public GameObject Puppy = null;

    //Property that determines how many puppy objects are spawned 
    public int PuppyCounter = 1;

    //empty gameobject that determines where in 3D space, they are spawned
    public Transform SpawnLocation = null;

    //Force applied to puppy objects
    public Vector3 RandomVelocity = new Vector3 (0, 0, 0); 

	void Start () //Initialize
    {
        //this object that this script is being attached to(gameobject), gets connected to TabUpdated event
        gameObject.Connect(Events.TabUpdatedEvent, OnTabUpdated);
	}

	/*
    */
    public void OnTabUpdated(EventData data)
    {
        //put the data received from the event into a variable and type cast it as a FloatEvent
        var TestData = data as FloatEvent;
        //print(TestData.value); Value is from 1 to almost 0

        //If the value received from the event is greater than 0.95, AND the puppies havent been spawned yet
        if (TestData.value > 0.95 && PuppiesAreSpawned == false)
        {
            //Call PuppyCannon
            PuppyCannon();
            //Flip the bool so that puppies are called once and not every frame
            PuppiesAreSpawned = true;
        }
    }

	// Update is called once per frame
	void Update ()
    {
	}

    /* Instantiate the puppy gameobject x number of times and apply velocity to it.
    */
    public void PuppyCannon()
    {
        for(int i = 0; i < PuppyCounter; ++i)
        {
            //"GameObject.Instantiate" does not instantiate a gameobject ._. wut. I have to typecast result as gameobj
            var puppy = GameObject.Instantiate(Puppy, SpawnLocation.position, Quaternion.identity) as GameObject;
            //RandomVelocity = new Vector3(Random.Range(0, 5), Random.Range(0, 7), Random.Range(-5, 3));
            puppy.GetComponent<Rigidbody>().velocity = (RandomVelocity);
        }
    }
}
