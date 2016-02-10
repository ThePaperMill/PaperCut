using UnityEngine;
using System.Collections;

public class ActivateChildrenOnCommand : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        EventSystem.GlobalHandler.Connect(Events.CatchFire, ActivateAnimationAndShader);
    }
	
	// Update is called once per frame
	void Update () {
	}
    public void ActivateAnimationAndShader(EventData data)
    {
        //turn on the shader in the children
        BroadcastMessage("TurnOn");
        //activate theanimation
        Animator myanim = gameObject.GetComponent<Animator>();
        //GetComponent<Animation>().Play();
        myanim.SetBool("turnson", true);


    }
}
