using UnityEngine;
using System.Collections;

public class PullTab_ModifyScale : MonoBehaviour
{
    public Vector3 TargetScale = new Vector3(1, 1, 1);
    Vector3 StartScale = new Vector3();

    void Start() //Initialize
    {
        gameObject.Connect(Events.TabUpdatedEvent, OnTabUpdated);
        StartScale = transform.localScale;
    }


    public void OnTabUpdated(EventData data)
    {
        var TestData = data as FloatEvent;

        transform.localScale = Vector3.Lerp(StartScale, TargetScale, TestData.value);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
