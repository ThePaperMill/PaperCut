using UnityEngine;
using System.Collections;

public class PullTab_MoveToPoint : MonoBehaviour
{
    public Vector3 TargetPosition = new Vector3();
    Vector3 StartPosition = new Vector3();

    void Start() //Initialize
    {
        gameObject.Connect(Events.TabUpdatedEvent, OnTabUpdated);
        StartPosition = transform.position;
    }


    public void OnTabUpdated(EventData data)
    {
        var TestData = data as FloatEvent;

        transform.position = Vector3.Lerp(StartPosition, TargetPosition, TestData.value);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
