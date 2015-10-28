using UnityEngine;
using System.Collections;
using ActionSystem.Internal;
using ActionSystem; //You either must say "using ActionSystem" or put "ActionSystem." in front of anything having to do with actions.
using System.Reflection;
using System; //If you are using System, you will need to put "ActionSystem." in front of any calls on the "Action" class.

public class TestScript : MonoBehaviour
{
    //TYPES:
    //ActionProperty - Interpolates a single given value over time, with a given ease.
    //ActionDelay - Does nothing for the specified amount of time.
    //ActionCall - Calls the given non-returning function, using the specified parameters.
    //ActionReturnCall - Calls the given returning function, using the specified parameters.
    //ActionSequence - Calls each action stored sequencially until no more actions remain.
    //ActionGroup - Calls all the actions stored at the same time until they have all been completed.
    

    //The easiest way to use the ActionSystem is to create a new ActionGroup (the master group) as a member variable.  
    ActionGroup Grp = new ActionGroup();
    //If you wish to get the return from the function called by an ActionReturnCall, you should also store the ActionReturnCall in a variable.
    //If you want to store several different types of ActionReturnCalls, you could make this a ActionBase and typecast it later.
    ActionReturnCall<int, int> ReturnCall;
    void Start()
    {
        //Create an ActionSequence to be run inside the master group.
        //A looping sequence will restart when it is finished and never end.
        var seq = ActionSystem.Action.Sequence(Grp, true);
        //Create a new property to be run inside the new sequence.
        //I have written a function to act a @this.Owner.Transform.Translation in Zilch. This function is "GetProperty" and takes in
        //To do the same thing, say "targetParent.GetProperty(x => x.target)".
        //In the below case: targetParent = this.gameObject.transform : x can be any random name : x.target = o.position.
        
        ActionSystem.Action.Property(seq, this.gameObject.transform.GetProperty(o => o.position), this.gameObject.transform.position + new Vector3(5, 0, 0), 2, Ease.CircInOut);
        ActionSystem.Action.Property(seq, this.gameObject.transform.GetProperty(o => o.localScale), new Vector3(20, 56, 2), 2, Ease.ExpoInOut);
        //Action.Delay is exactly like in Zero.
        ActionSystem.Action.Delay(seq, 1.5);
        //Action.Call takes in the function being called, with the following parameters being what the function takes in.
        //There is a current maximum of 4 paramaters.
        ActionSystem.Action.Call(seq, Hello, 26, 42);

        //Here I am creating a group to run within the ActionSequence.
        //A looping ActionGroup will loop until it is destroyed or told to stop.
        //Putting a looping ActionGroup within an ActionSequence will cause the sequence to get stuck on the group.
        var grp = ActionSystem.Action.Group(seq, false);
        //These two ActionProperties will be run at the same time.
        ActionSystem.Action.Property(grp, this.gameObject.transform.GetProperty(o => o.position), this.gameObject.transform.position, 3, Ease.QntInOut);
        ActionSystem.Action.Property(grp, this.gameObject.transform.GetProperty(o => o.eulerAngles), new Vector3(180, 0, 180), 3, Ease.QntInOut);

        //In order to get the return back from a ReturnCall, you must specify in the template paramaters what the function takes in AND the return type.
        //You must then store the new ReturnCall somewhere to be accesed later.
        //There is a current maximum of 4 paramaters.
        ReturnCall = ActionSystem.Action.ReturnCall<int, int>(grp, Hello, 20);

        //After the call is completed, you can access ReturnCall.Return to get the value.
        ActionSystem.Action.Call(seq, PrintReturn);
        
        //Clearing an ActionSequence or ActionGroup will STOP all the actions and subactions in the Sequence/Group.
        //Because C# is refrence counted, an action will not be destroyed if you are storing it in a variable.
        //This is perfectly fine, and will not cause any issues.
    }

    // Update is called once per frame
    void Update()
    {
        //You MUST call update on the master group every rame and pase in the desired for of Delta Time
        Grp.Update(Time.smoothDeltaTime);
    }

    public void Hello(int a, int b)
    {
        Debug.Log("Hello WORLD! " + a + " " + b);
    }

    public int Hello(int a)
    {
        Debug.Log("Hello WORLD again! " + a);
        return a + 10;
    }

    public void PrintReturn()
    {
        Debug.Log(ReturnCall.Return);
    }
}

