using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HingeJoint))]
[ExecuteInEditMode]
public class EditorWiggle : MonoBehaviour {
    HingeJoint hin = null;

    // Use this for initialization
    void Start () {

        hin = gameObject.GetComponent<HingeJoint>();
        if (hin.anchor.y >= 0)
        {
            //print(gameObject.name);
            hin.anchor = new Vector3(hin.anchor.x, -hin.anchor.y, hin.anchor.z);
        }

        if (hin.useSpring == false)
        {
            hin.useSpring = true;
        }

        JointSpring hingeSpring = hin.spring;


        if (hingeSpring.spring <= 0)
        {
            hingeSpring.spring = 20f;
        }
        if (hingeSpring.damper <= 0)
        {
            hingeSpring.damper = 1;
        }


        hin.spring = hingeSpring;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
