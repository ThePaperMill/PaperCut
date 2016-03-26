/****************************************************************************/
/*!
\file   PullTabChild.cs
\author Ian Aemmer and Joshua Biggs.
\brief  
    Description of script
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;

public class PullTabChildTransform : MonoBehaviour
{
    public Curve EasingCurve = Ease.Linear;
    //public bool AdditiveRotation = true;
    public Vector3 TransformVector = Vector3.zero;
    Vector3 StartVector = Vector3.zero;
	// Use this for initialization
	void Start ()
    {
        StartVector = transform.localPosition;
        //if(AdditiveRotation)
        //{
        //    BendVector += StartVector;
        //}
        transform.root.Connect(Events.TabUpdatedEvent, UpdateRot);
	}
    public void UpdateRot(EventData slerptimer)
    {

        //var vector = Vector3.LerpUnclamped(StartVector, BendVector, ((FloatEvent)slerptimer).value);
        //transform.localRotation.eulerAngles.Set(vector.x, vector.y, vector.z); //Vector3.LerpUnclamped(Quaternion.Euler(StartVector), Quaternion.Euler(BendVector), slerptimer);
        //transform.rotation = Quaternion.Euler(ActionSystem.ActionMath<Vector3>.QuadInOut(((FloatEvent)slerptimer).value, StartVector, BendVector, 1));
        transform.localPosition = EasingCurve.Sample<Vector3>(((FloatEvent)slerptimer).value, StartVector,TransformVector, 1);
    }
}
