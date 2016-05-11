using UnityEngine;
using System.Collections;

public class AnimationTest : MonoBehaviour {

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        return;

        if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("PullLeft", true);
        }
        else
        {
            anim.SetBool("PullLeft", false);

        }
        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("PullRight", true);
        }
        else
        {
            anim.SetBool("PullRight", false);

        }
    }
}
