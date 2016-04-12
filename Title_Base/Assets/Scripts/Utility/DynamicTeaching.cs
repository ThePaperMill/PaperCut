/****************************************************************************/
/*!
\file  DynamicTeaching.cs
\author Steven Gallwas 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;

public class DynamicTeaching : MonoBehaviour
{
    public bool CheckMove = false;
    public bool CheckJump = false;
    public bool CheckInteract = false;

    bool Moved = false;
    bool Jumped = false;
    bool Interacted = false;

    float WaitTimer = 0.0f;

    public float WaitDelay = 5.0f;

    public GameObject DropDown = null;
    public Vector3 FinalPosition = new Vector3();
    public bool DroppedDown = false;

    ActionSequence grp = new ActionSequence();
    Vector3 StartingPosition = new Vector3();

    bool Finished = false;

    // Use this for initialization
    void Start ()
    {
        if(DropDown)
        StartingPosition = DropDown.transform.position;
	}
	
    void ActivateText()
    {
        if(DroppedDown)
        {
            return;
        }

        DroppedDown = true;

        if(DropDown)
        {
            DropDown.SetActive(true);
            ActionSequence temp = Action.Sequence(grp);
            Action.Property(temp, DropDown.transform.GetProperty(o => o.localPosition), FinalPosition, 1.0f, Ease.Linear);
        }
    }

    void HideText()
    {
        if (DropDown)
        {
            Finished = true;
            ActionSequence temp = Action.Sequence(grp);
            Action.Property(temp, DropDown.transform.GetProperty(o => o.localPosition), StartingPosition, 0.50f, Ease.Linear);
            Action.Delay(temp, 0.25f);
            Action.Call(temp, DeactivateText);
        }
    }

    void DeactivateText()
    {
        DropDown.SetActive(false);
    }

	// Update is called once per frame
	void Update ()
    {
        WaitTimer += Time.deltaTime;

        if (CheckMove)
        {
            if (InputManager.GetSingleton.IsInputDown(GlobalControls.AnyMovement) || InputManager.GetSingleton.IsLeftStickTriggered())
            {
                Moved = true;
            }
        }
        if(CheckInteract)
        {
            if (InputManager.GetSingleton.IsInputDown(GlobalControls.InteractKeys))
            {
                Interacted = true;
            }
        }
        if(CheckJump)
        {
            if (InputManager.GetSingleton.IsInputDown(GlobalControls.JumpKeys))
            {
                Jumped = true;
            }
        }

        if (WaitTimer > WaitDelay)
        {
            if (CheckMove && Moved == false)
            {
                ActivateText();
            }
            if (CheckJump && Jumped == false)
            {
                ActivateText();
            }
            if (CheckInteract && Interacted == false)
            {
                ActivateText();
            }
        }

        if(DroppedDown && Finished == false)
        {
            if (CheckMove && Moved == true)
            {
                HideText();          
            }
            if (CheckJump && Jumped == false)
            {
                HideText();
            }
            if (CheckInteract && Interacted == false)
            {
                HideText();
            }
        }

        grp.Update(Time.deltaTime);
    }
}
