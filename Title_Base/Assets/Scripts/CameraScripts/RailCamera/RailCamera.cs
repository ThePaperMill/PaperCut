using UnityEngine;
using System.Collections;

public class RailCamera : MonoBehaviour
{
    public Rail DefaultRail;
    public float DefaultMoveSpeed = 2f;
    public Vector3 DefaultLookAngle = new Vector3(20,0,0);

    [HideInInspector]
    public Rail CurrentRail;
    [HideInInspector]
    public float CurrentMoveSpeed;
    [HideInInspector]
    public Vector3 TargetLookAngle;

    private GameObject TargetObject;         // The object we want the camera to focus on
    private GameObject DefaultTarget;       // Variable for storing default value of TargetObject
    private Vector3 CurrentLookAngle;

    // Use this for initialization
    void Start ()
    {
        CurrentRail = DefaultRail;
        CurrentMoveSpeed = DefaultMoveSpeed;
        TargetLookAngle = DefaultLookAngle;
    }
	
	// Update is called once per frame
	public void Update ()
    {
        if (TargetObject == null)
        {
            TargetObject = GameObject.FindGameObjectWithTag("Player");

            DefaultTarget = TargetObject;       // Store the default value of TargetObject

            return;
        }

        transform.position = Vector3.Lerp(transform.position, CurrentRail.RailPosition(TargetObject.transform.position), Time.deltaTime * CurrentMoveSpeed);
        
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, TargetLookAngle, Time.deltaTime * CurrentMoveSpeed);
    }

    public void ReturnToDefaults()
    {
        CurrentRail = DefaultRail;
        CurrentMoveSpeed = DefaultMoveSpeed;
        TargetLookAngle = DefaultLookAngle;
    }
}