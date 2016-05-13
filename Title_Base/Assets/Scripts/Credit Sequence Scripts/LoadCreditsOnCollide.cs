/****************************************************************************/
/*!
\file   LoadCreditsOnCollide.cs
\author Ian Aemmer
\brief  
    This file contains the implementation of a ladder script 

    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using ActionSystem;

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadCreditsOnCollide : MonoBehaviour
{
    public string SceneName;

    public GameObject passenger = null;

    bool Activate = false;

    public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    private ActionGroup grp = new ActionGroup();
    public float EaseTime = 5.0f;
    public Vector3 finalPos = new Vector3(0, 0, 0);//.zero;

    // Use this for initialization
    void Start ()
    {

        gameObject.Connect(Events.InteractedWith, OnInteractedWith);
        // public static readonly String InteractedWith = "InteractedWithEvent";
    }

// Update is called once per frame
void Update ()
	{
	
        if(Activate)
        {
            //I need to enable my shake script
            //print("I am activated");

            //I want my

            

            grp.Update(Time.smoothDeltaTime);

        }

        

    }

    void OnInteractedWith(EventData myevent)
    {
        //Destroy(col.gameObject);
        if (passenger != null)
        {
            passenger.SetActive(true);
            GameObject.Find("DynamicPlayer(Clone)").SetActive(false);
            this.Activate = true;
            //Destroy(gameObject.GetComponent<Interactable>());
            GameObject.Find("Highlight(Clone)").SetActive(false);//.GetComponent<MeshRenderer>().enabled = false;
            //Destroy(GameObject.Find("Highlight(Clone)").GetComponent<MeshRenderer>())
;            ActivateAnimation();
        }
    }

    void ActivateAnimation()
    {
        Vector3 startpos = transform.localPosition;

        var seq = Action.Sequence(grp);
        //var grp = Action.Group(grp);
        
        Action.Property(seq, passenger.transform.GetProperty(ObjTransform => ObjTransform.localPosition), passenger.transform.localPosition + new Vector3(0,0.4f,0), EaseTime, Curve);
        //this is getting called, but it never ends or moves onto the next function
        Action.Call(seq, JoshIHopeThisWorksOrYoureFired);
        Action.Property(seq, gameObject.transform.GetProperty(ObjTransform => ObjTransform.localPosition), startpos + new Vector3(0, 15, 0), 5, Curve);
		Action.Call(seq, LoadCreditLevel);
    }

    void JoshIHopeThisWorksOrYoureFired()
    {
        var soundEmitter = GetComponent<FMOD_StudioEventEmitter>();

        if (soundEmitter)
        {
            soundEmitter.StartEvent();
        }

        //gameObject.GetComponent<Shake>().enabled = true;
        transform.FindChild("GameObject").GetComponent<Shake>().enabled = true;
        transform.FindChild("Fire").gameObject.SetActive(true);

    }
    void LoadCreditLevel()
    {
        //SceneManager.LoadScene(SceneName);
        LevelTransitionManager.GetSingleton.ChangeLevel(SceneName);
    }
}
