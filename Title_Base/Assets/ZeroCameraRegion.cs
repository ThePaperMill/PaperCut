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

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider otherObj)
    {
        // If a new Target is specified, change it on the main camera
        if (NewTarget != null)
        {
            Camera.main.GetComponent<ZeroCamera>().TargetObject = NewTarget;
        }
        // If a new Distance is specified, change it on the main camera
        if (NewDistance != new Vector3(0,0,0))
        {
            Camera.main.GetComponent<ZeroCamera>().FollowDistance = NewDistance;
        }
        // if a new LerpSpeed is specified, change it on the main camera.
        if (NewLerp != 0.0f)
        {
            Camera.main.GetComponent<ZeroCamera>().LerpSpeed = NewLerp;
        }
    }

    void OnTriggerExit(Collider otherObj)
    {
        // upon leaving the region, reset all values to their camera defaults.
        Camera.main.GetComponent<ZeroCamera>().ResetToDefaults();
    }
}
