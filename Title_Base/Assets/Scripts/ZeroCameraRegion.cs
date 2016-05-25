/****************************************************************************/
/*!
\author 
© 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

public class ZeroCameraRegion : MonoBehaviour
{
    public GameObject NewTarget;            // A gameobject to be a new target of the camera
    public Vector3 NewDistance;             // The new distance away of the camera from the target
    public float NewLerp;                   // The new speed at which the camera should focus on the target
    public bool UseLastInfo = false;
    Vector3 LastDistance;
    GameObject LastTarget;
    float LastLerp;
	// Use this for initialization
	void Start ()
    {
       
    }
	
	// Update is called once per frame
	void Update ()
    {
        //allows us to see the vector during gameplay (only works in scene view)
        if(Application.isEditor)
        {
            Debug.DrawLine(transform.position, transform.position + NewDistance, Color.red);

        }
    }

    void OnTriggerEnter(Collider otherObj)
    {
        if(otherObj.tag != "Player")
        {
            return;
        }
        if(!NewTarget)
        {
            NewTarget = GameObject.FindGameObjectWithTag("Player");
        }
        var cam = Camera.main.GetComponent<ZeroCamera>();
        LastDistance = cam.FollowDistance;
        LastTarget = cam.TargetObject;
        LastLerp = cam.LerpSpeed;
        // If a new Target is specified, change it on the main camera
        if (NewTarget != null)
        {
            cam.TargetObject = NewTarget;
        }
        // If a new Distance is specified, change it on the main camera
        if (NewDistance != new Vector3(0,0,0))
        {
            cam.FollowDistance = NewDistance;
        }
        // if a new LerpSpeed is specified, change it on the main camera.
        if (NewLerp != 0.0f)
        {
            cam.LerpSpeed = NewLerp;
        }
    }

    void OnTriggerExit(Collider otherObj)
    {
        if (otherObj.tag != "Player")
        {
            return;
        }
        var cam = Camera.main.GetComponent<ZeroCamera>();
        if (UseLastInfo)
        {
            cam.FollowDistance = LastDistance;
            cam.TargetObject = LastTarget;
            cam.LerpSpeed = LastLerp;
        }
        else
        {
            // upon leaving the region, reset all values to their camera defaults.
            cam.ResetToDefaults();
        }
    }
}
