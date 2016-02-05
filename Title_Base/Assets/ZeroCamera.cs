using UnityEngine;
using System.Collections;

public class ZeroCamera : MonoBehaviour
{
    public GameObject TargetObject;         // The object we want the camera to focus on
    public Vector3 FollowDistance;          // The distance away from the target object we want the camera to be
    public float LerpSpeed;                 // The speed at which the camera moves to its new location.

    private GameObject DefaultTarget;       // Variable for storing default value of TargetObject
    private Vector3 DefaultFollow;          // Variable for storing default value of FollowDistance
    private float DefaultLerp;              // Variable for storing default value of LerpSpeed
    private Vector3 velocity = Vector3.zero;
    private Vector3 LookAtPoint;

	// Use this for initialization
	void Start ()
    {
        DefaultTarget = TargetObject;       // Store the default value of TargetObject
        DefaultFollow = FollowDistance;     // Store the default value of FollowDistance
        DefaultLerp = LerpSpeed;            // Store the default value of LerpSpeed
        LookAtPoint = TargetObject.transform.position;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
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