using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class FireParticleController : MonoBehaviour {

    //What do I need to do?
    //Each particle system adds itself to the FireParticleController on Runtime 
    //The Fire Particle Controller, when Activated (through the Activate Functtion, which flips the boolean)
    //When the boolean is true, if it was not true last frame, loops through all the particle systems in it's array of gameobjects and flips a coin. If heads, turns that system on.

    //When boolean is false, if it was not false last frame, loop through all particle systems and turn them off.

    public List<GameObject> FireEmitters;

    public bool FlameOn = false;
    public bool debugcontrol = true;

	// Use this for initialization
	void Start () {
        //Initialize the list "FireEmitters"
        FireEmitters = new List<GameObject>();
        //For each child this gameobject has,
        for(int i = 0; i < transform.childCount; ++i)
        {
            //grab the child
            GameObject mychild = this.gameObject.transform.GetChild(i).gameObject;
            //Add them to my list if they're a particle system
            if(mychild.GetComponent<MeshParticleEmitter>() != null)
            {
                FireEmitters.Add(mychild);
                mychild.SetActive(false);
            }
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(debugcontrol == true)
        {
            //print(Random.Range(0f, 1f));
            if (Input.GetKeyDown(KeyCode.A))
            {
                print("wat");
                SwapFlameOn();
            }
            if (Input.GetKey(KeyCode.S))
            {
                EditSize(-0.01f);
            }
            if (Input.GetKey(KeyCode.W))
            {
                EditSize(0.01f);
            }
        }


    }

    //SwapFlameOn turns the emitter on or not. When turning on, it only turns some of them on
    public void SwapFlameOn()
    {
        //if the flames are on, turn them off.
        if (FlameOn == true)
        {
            foreach (var child in FireEmitters)
            {
                child.SetActive(false);
            }
        }
        //Otherwise turn some of them on
        else
        {
            foreach (var child in FireEmitters)
            {
                //Flip a coin, if heads...
                if (Random.Range(0f, 1f) > 0.5)
                {
                    //turn that particular emitter on
                    child.SetActive(true);
                }
            }
        }
        FlameOn = !FlameOn;
    }
    //size should always be 1/10 of max emitter
    public void EditSize(float size)
    {

        foreach (var child in FireEmitters)
        {
            print(child.transform.localScale);
            child.transform.localScale += new Vector3(size, 0, size);
            print(child.transform.localScale);

            child.GetComponent<MeshParticleEmitter>().minEmission = child.transform.localScale.x * 50;
            child.GetComponent<MeshParticleEmitter>().maxEmission = child.transform.localScale.x * 100;

            //Max EMission should always be 1/10 of the Min Emitter Range
            //MeshParticleEmitter blah = child.GetComponent<MeshParticleEmitter>();
            //blah.
            //child.SetActive(false);
        }

    }

}
