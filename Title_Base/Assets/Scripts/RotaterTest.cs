/****************************************************************************/
/*!
\file   RotaterTest.cs
\author Ian Aemmer
\brief  
    rotaters things dawg.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

using ActionSystem.Internal;
using ActionSystem; //You either must say "using ActionSystem" or put "ActionSystem." in front of anything having to do with actions.
using System.Reflection;
using System; //If you are usung System, you will need to put "ActionSystem." in front of any calls on the "Action" class.


public class RotaterTest : MonoBehaviour
{
    public GameObject FlipSwitch = null;
    bool isTriggered = false;

    //PosGrp is an actiongroup covering the hover of the camera in every frame
    ActionGroup Grp = new ActionGroup();

    Vector3 StartingPos = Vector3.zero;
    public Vector3 ChangePos = new Vector3(-90f, 0f, 0f);

    // Use this for initialization
    void Start()
    {
        StartingPos = this.gameObject.transform.localEulerAngles;
        
        //RotateOnce();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(FlipSwitch != null)
        {
            OnCollideSwapBool dink = FlipSwitch.GetComponent<OnCollideSwapBool>();
            if(dink.IsOn == true && isTriggered == false)
            {
                isTriggered = true;
                RotateOnce();
            }
        }

        //print(this.gameObject.transform.localEulerAngles);
        Grp.Update(Time.smoothDeltaTime);
    }

    void RotateOnce()
    {
        //Vector3 newV = new Vector3(-90f, 0f, 0f);

        //Declare a new sequence using AimGrp
        var seq = ActionSystem.Action.Sequence(Grp);
        //Interpolate the X position of the AimTracker so that it approaches zero over the course of Timer
        //ActionSystem.Action.Property(seq, this.gameObject.transform.GetProperty(x => x.localPosition), Vector3.zero, 5.0f, Ease.QuadInOut);
        ActionSystem.Action.Property(seq, this.gameObject.transform.GetProperty(x => x.localEulerAngles), ChangePos+StartingPos, 2.0f, Ease.QuadIn);
    }
}
