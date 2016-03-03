/****************************************************************************/
/*!
\author 
© 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

public class TurnOnFire : MonoBehaviour {

    public GameObject Myparticlastieytstm = null;
	// Use this for initialization
	void Start () {
        EventSystem.GlobalHandler.Connect(Events.CatchFire, TurnonFire);
    }

    // Update is called once per frame
    void Update () {
	
	}
    public void TurnonFire(EventData data)
    {
        //turn on the shader in the children
        //BroadcastMessage("TurnOn");
        //activate theanimation
        //Animator myanim = gameObject.GetComponent<Animator>();
        //GetComponent<Animation>().Play();
        //myanim.SetBool("turnson", true);
        GameObject tempgame = (GameObject)Instantiate(Myparticlastieytstm, transform.position, Quaternion.identity);
        //print("hello");

    }
}
