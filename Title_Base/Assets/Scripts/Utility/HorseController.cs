using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ActionSystem;
public class HorseController : MonoBehaviour
{
    public float HorseSpeed = 3;
    GameObject Camera = null;
    GameStateControlledCamera CamScript = null;
    Rigidbody Body = null;
    public List<Transform> Nodes = new List<Transform>(1);
    // Use this for initialization
    List<Transform>.Enumerator CurrentNode = new List<Transform>.Enumerator ();

    public float Duration = 2.0f;

    
	void Start ()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        if(!Camera)
        {
            throw new System.Exception("This level needs a camera!");
        }
        CamScript = Camera.GetComponent<GameStateControlledCamera>();
        CamScript.transform.position.Set(transform.position.x, CamScript.transform.position.y, CamScript.transform.position.z);
        Camera.transform.SetParent(transform, true);
        CamScript.AimTarget = gameObject;
        CamScript.PosTarget = gameObject;
        CamScript.enabled = false;
        Camera.transform.localPosition.Set(0, CamScript.transform.position.y, CamScript.transform.position.z);
        Camera.transform.LookAt(gameObject.transform);
        
        Body = GetComponent<Rigidbody>();
        CurrentNode = Nodes.GetEnumerator();
        LookAtNextNode();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(InputManager.GetSingleton.IsInputTriggered(GlobalControls.JumpKeys))
        {
            Body.AddForce(0, 300, 0);
        }
        var directionVec = CurrentNode.Current.transform.position - transform.position;
        directionVec.y = 0;
        var mag = directionVec.sqrMagnitude;
        if (mag < 4)
        {
            LookAtNextNode();
        }
        transform.right = ActionMath.QuadInOut<Vector3>(Time.smoothDeltaTime * 3, transform.right, directionVec, 1);
        transform.position += transform.right * HorseSpeed * Time.smoothDeltaTime;
	}

    void ActivateHorse()
    {

    }

    public void LookAtNextNode()
    {
        CurrentNode.MoveNext();
        
        

        //transform.for(CurrentNode.Current.transform);
        //transform.eulerAngles -= new Vector3(0, 90, 0);
    }
}
