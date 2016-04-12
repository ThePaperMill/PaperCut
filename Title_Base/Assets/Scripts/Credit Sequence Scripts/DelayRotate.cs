using UnityEngine;
using System.Collections;

public class DelayRotate : MonoBehaviour {

    Vector3 rotationtracker = Vector3.zero;
    public Vector3 AddVector = Vector3.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        rotationtracker += AddVector;

        transform.localRotation = Quaternion.Euler(rotationtracker);
	}
}
