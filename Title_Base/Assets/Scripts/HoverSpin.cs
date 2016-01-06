using UnityEngine;
using ActionSystem;
using System.Collections;

public class HoverSpin : MonoBehaviour
{
    private bool JumpPressed;
    private bool JumpReleased;
    private bool Grounded;
    private bool JumpHeld;

    public Vector3 Prone = new Vector3();

    int JumpTimer;
    ActionGroup grp = new ActionGroup();
    Rigidbody RB = new Rigidbody();

    // Use this for initialization
    void Start ()
    {
        JumpTimer = 0;
        Grounded = this.GetComponent<CustomDynamicController>().OnGround;
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        Grounded = this.GetComponent<CustomDynamicController>().OnGround;
        JumpPressed = InputManager.GetSingleton.IsInputTriggered(GlobalControls.JumpKeys);
        JumpReleased = InputManager.GetSingleton.IsInputReleased(GlobalControls.JumpKeys);

        Spin();
            
        if (Grounded == true)
        {
            JumpTimer = 0;
        }
    }

    //This should be renamed to Prone or something
    void Spin()
    {
        bool JumpHeld = InputManager.GetSingleton.IsInputDown(GlobalControls.JumpKeys);
        //print(JumpReleased);
        if (Grounded == false && JumpHeld == true) 
        {
            ++JumpTimer;
        }
        //print(JumpTimer);

        if(JumpTimer == 24)
        {
            Prone = new Vector3(90.0f, 0, 0);
            var seq = ActionSystem.Action.Sequence(grp);
            Action.Property(seq, this.transform.GetProperty(x => x.localEulerAngles), Prone, 0.05f, Ease.Linear);

            RB = this.GetComponent<Rigidbody>();

            RB.velocity *= 0; //new Vector3(RB.velocity.x 0, RB.velocity.y * 0, RB.velocity.z);

            //this.transform.localEulerAngles = Prone;
            //print(this.transform.localEulerAngles);
        }
        grp.Update(Time.smoothDeltaTime);

        if (JumpTimer == 0)
        {
            Prone = new Vector3(0, 0, 0);
            this.transform.localEulerAngles = Prone;
        }
    }
}
