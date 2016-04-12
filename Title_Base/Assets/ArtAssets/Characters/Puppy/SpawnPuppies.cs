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

    bool PuppiesAreSpawned = false;

    public GameObject Puppy = null;

    public int PuppyCounter = 1;

    public Transform SpawnLocation = null;

    public Vector3 RandomVelocity = new Vector3 (0, 0, 0); 
	// Use this for initialization
	void Start ()
    {
        
        gameObject.Connect(Events.TabUpdatedEvent, OnTabUpdated);
	}
	
    public void OnTabUpdated(EventData data)
    {
        var TestData = data as FloatEvent;
        //print(TestData.value); Value is from 1 to almost 0

        if (TestData.value > 0.95 && PuppiesAreSpawned == false)
        {
            PuppyCannon();
            PuppiesAreSpawned = true;
        }
    }

	// Update is called once per frame
	void Update ()
    {
	}

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
