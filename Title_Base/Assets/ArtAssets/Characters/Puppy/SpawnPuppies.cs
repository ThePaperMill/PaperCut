using UnityEngine;
using System.Collections;

public class SpawnPuppies : MonoBehaviour
{

    public bool PuppiesAreSpawned = false;

    public GameObject Puppy = null;

    public int PuppyCounter = 1;

    public Transform SpawnLocation = null;

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
            var puppy = GameObject.Instantiate(Puppy, SpawnLocation.position, Quaternion.identity);
        }
        
    }
}
