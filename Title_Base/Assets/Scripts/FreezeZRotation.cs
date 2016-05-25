using UnityEngine;
using System.Collections;

public class FreezeZRotation : MonoBehaviour {

    float FrozenZRotation = 0f;
	// Use this for initialization
	void Start () {

        FrozenZRotation = transform.rotation.eulerAngles.z;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newrot = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, FrozenZRotation);
        transform.rotation = Quaternion.Euler(newrot);
	}
}
