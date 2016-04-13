/****************************************************************************/
/*!
\file   PullTabChild.cs
\author Ian Aemmer and Joshua Biggs.
\brief  
    Description of script, left abandoned by its creators to never properly
    describe the script it was left in, for fate is cruel and unforgiving.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class PullTabChild : MonoBehaviour
{

    //public bool AdditiveRotation = true;
    public Vector3 BendVector = Vector3.zero;
    Vector3 StartVector = Vector3.zero;
    bool PoppedUp = false;
	// Use this for initialization
	void Awake ()
    {
        StartVector = transform.localRotation.eulerAngles;
        //if(AdditiveRotation)
        //{
        //    BendVector += StartVector;
        //}
        transform.root.gameObject.Connect(Events.TabUpdatedEvent, UpdateRot);
	}
    public void UpdateRot(EventData slerptimer)
    {
        FloatEvent eventData = slerptimer as FloatEvent;
        //Debug.Log("SASAD");
        //var vector = Vector3.LerpUnclamped(StartVector, BendVector, ((FloatEvent)slerptimer).value);
        //transform.localRotation.eulerAngles.Set(vector.x, vector.y, vector.z); //Vector3.LerpUnclamped(Quaternion.Euler(StartVector), Quaternion.Euler(BendVector), slerptimer);
        //transform.rotation = Quaternion.Euler(ActionSystem.ActionMath<Vector3>.QuadInOut(((FloatEvent)slerptimer).value, StartVector, BendVector, 1));
        transform.localRotation = ActionSystem.ActionMath.QuadInOut<Quaternion>(eventData.value, Quaternion.Euler(StartVector), Quaternion.Euler(BendVector), 1);
        this.gameObject.DispatchEvent("RotUpdated");
    }
}
