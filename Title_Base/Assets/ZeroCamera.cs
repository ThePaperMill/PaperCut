/****************************************************************************/
/*!
\author 
© 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

public class ZeroCamera : MonoBehaviour
{
    public Vector3 FollowDistance;          // The distance away from the target object we want the camera to be
    public float LerpSpeed;                 // The speed at which the camera moves to its new location.
    public GameObject TargetObject;         // The object we want the camera to focus on

    private GameObject DefaultTarget;       // Variable for storing default value of TargetObject
    private Vector3 DefaultFollow;          // Variable for storing default value of FollowDistance
    private float DefaultLerp;              // Variable for storing default value of LerpSpeed
    //private Vector3 velocity = Vector3.zero;
    private Vector3 LookAtPoint;

    public GameObject StartingLookTarget = null;

	// Use this for initialization
	void Start ()
    {
        DefaultFollow = FollowDistance;     // Store the default value of FollowDistance
        DefaultLerp = LerpSpeed;            // Store the default value of LerpSpeed

        if(TargetObject)
            LookAtPoint = TargetObject.transform.position;
        else if(StartingLookTarget)
        {
            LookAtPoint = StartingLookTarget.transform.position;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (GamestateManager.GetSingleton.CurState == GAME_STATE.GS_CINEMATIC)
        {
            return;
        }

        if (TargetObject == null)
        {
            TargetObject = GameObject.FindGameObjectWithTag("Player");

            DefaultTarget = TargetObject;       // Store the default value of TargetObject

            return;
        }
        //print(TargetObject.name);

        // Lerp the cameras position towards the TargetObjects, plus offset of FollowDistance, at LerpSpeed
        transform.position = Vector3.Lerp(transform.position, TargetObject.transform.position + FollowDistance, LerpSpeed * Time.fixedDeltaTime);

        LookAtPoint = Vector3.Lerp(LookAtPoint, TargetObject.transform.position, Time.smoothDeltaTime * 3);

        // Constantly stare directly at the Target Object
        transform.LookAt(LookAtPoint);
	}

    /* Called by ZeroCameraRegion when leaving a region that alters base values */
    public void ResetToDefaults()
    {
        TargetObject = DefaultTarget;       // Set TargetObject back to default, in case it has changed.
        FollowDistance = DefaultFollow;     // Set FollowDistance back to default, in case it has changed.
        LerpSpeed = DefaultLerp;            // Set LerpSpeed back to default, in case it has changed.
    }
}