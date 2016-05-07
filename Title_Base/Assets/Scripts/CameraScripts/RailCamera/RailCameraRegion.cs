using UnityEngine;
using System.Collections;

public class RailCameraRegion : MonoBehaviour
{
    public Rail NewRail;
    public float NewSpeed;
    public Vector3 NewAngle;

    void OnTriggerEnter(Collider otherObj)
    {
        var cam = Camera.main.GetComponent<RailCamera>();

        // If a new Target is specified, change it on the main camera
        if (NewRail != null)
        {
            cam.CurrentRail = NewRail;
        }
        // If a new Distance is specified, change it on the main camera
        if (NewAngle != new Vector3(0, 0, 0))
        {
            Camera.main.transform.eulerAngles = NewAngle;
        }
        // if a new LerpSpeed is specified, change it on the main camera.
        if (NewSpeed != 0.0f)
        {
            cam.CurrentMoveSpeed = NewSpeed;
        }
    }

    void OnTriggerExit(Collider otherObj)
    {
        var cam = Camera.main.GetComponent<RailCamera>();

        cam.ReturnToDefaults();
    }
}