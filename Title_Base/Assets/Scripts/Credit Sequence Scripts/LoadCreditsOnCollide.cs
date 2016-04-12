/****************************************************************************/
/*!
\file   LoadCreditsOnCollide.cs
\author Ian Aemmer
\brief  
    This file contains the implementation of a ladder script 

    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;




public class LoadCreditsOnCollide : MonoBehaviour {
    public string SceneName;

    public GameObject passenger = null;

    bool Activate = false;

    // Use this for initialization
    void Start ()
    {

        gameObject.Connect(Events.InteractedWith, OnInteractedWith);
        // public static readonly String InteractedWith = "InteractedWithEvent";
    }

// Update is called once per frame
void Update () {
	
        if(Activate)
        {
            //I need to enable my shake script








        }
	}
    /*
    void OnCollisionEnter(Collision col)
    {
        //print(col.gameObject.name);
        if(col.gameObject.name == "DynamicPlayer(Clone)")
        {
            //LoadSceneMode.)
            //SceneManager.LoadScene(SceneName);

        }

    }*/

    void OnInteractedWith(EventData myevent)
    {
        //Destroy(col.gameObject);
        if (passenger != null)
        {
            passenger.SetActive(true);
            GameObject.Find("DynamicPlayer(Clone)").SetActive(false);
            this.Activate = true;
        }
    }
}
