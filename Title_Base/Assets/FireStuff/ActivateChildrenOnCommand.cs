using UnityEngine;
using System.Collections;

public class ActivateChildrenOnCommand : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	    if(Input.GetKeyDown("up"))
        {
            //ActivateAnimationAndShader();
            
            /*
            var allTestMaterialChangers : TestMaterialChanger[];
            AllTestMaterialChangers = gameobject.GetComponentsInChildren.<TestMaterialChanger>();
            for(var childTestMatChanger : TestMaterialChanger in allTestMaterialChangers)
            {
                print("what is happening");
            }*/



        }
	}
    public void ActivateAnimationAndShader()
    {
        //turn on the shader in the children
        BroadcastMessage("TurnOn");
        //activate theanimation
        Animator myanim = gameObject.GetComponent<Animator>();
        //GetComponent<Animation>().Play();
        myanim.SetBool("turnson", true);
    }
}
