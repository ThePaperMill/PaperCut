using UnityEngine;
using System.Collections;

public class HideDuringPause : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(GamestateManager.GetSingleton.CurState == GAME_STATE.GS_PAUSE)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
	}
}
