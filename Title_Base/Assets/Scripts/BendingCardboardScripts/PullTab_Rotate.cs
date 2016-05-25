using UnityEngine;
using System.Collections;

public class PullTab_Rotate : MonoBehaviour
{
     public Vector3 TargetRotation = new Vector3();
     Quaternion StartingRotation = new Quaternion();

    void Start() //Initialize
    {
        gameObject.Connect(Events.TabUpdatedEvent, OnTabUpdated);
        StartingRotation = transform.rotation;
    }


    public void OnTabUpdated(EventData data)
    {
        var TestData = data as FloatEvent;

        Quaternion TargetRot = Quaternion.Euler(TargetRotation);

        transform.rotation = Quaternion.Lerp(StartingRotation, TargetRot, TestData.value);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
