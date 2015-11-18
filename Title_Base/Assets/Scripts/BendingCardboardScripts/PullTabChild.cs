using UnityEngine;
using System.Collections;

public class PullTabChild : MonoBehaviour {

    public Vector3 BendVector = Vector3.zero;
    Vector3 StartVector = Vector3.zero;
	// Use this for initialization
	void Start () {
        StartVector = transform.localRotation.eulerAngles;
	}
    public void UpdateRot(float slerptimer)
    {
        transform.localRotation = Quaternion.Slerp(Quaternion.Euler(StartVector), Quaternion.Euler(BendVector), slerptimer);
    }
}
