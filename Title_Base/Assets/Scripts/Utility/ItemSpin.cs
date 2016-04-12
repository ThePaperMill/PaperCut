using UnityEngine;
using System.Collections;
using ActionSystem;

public class ItemSpin : MonoBehaviour
{
    public bool Bounce = false;

    public float speed = 20f;

    float BounceAmount = 0.015f;

    [HideInInspector]
    public Vector3 StartingPostion = new Vector3();

    public float HeightOffset { get; set; }

    void Start()
    {
        StartingPostion = transform.position;

        HeightOffset = 0.0f;

        if (Bounce)
        {
            ActionSequence temp = Action.Sequence(this.GetActions());
            Action.Property(temp, this.GetProperty(o => o.HeightOffset), BounceAmount, 1.0f, Ease.Linear);
            
            Action.Delay(temp, 0.350f);
            Action.Call(temp, ActionBounce);
        }

    }

    void ActionBounce()
    {
        BounceAmount *= -1;
        ActionSequence temp = Action.Sequence(this.GetActions());
        Action.Property(temp, this.GetProperty(o => o.HeightOffset), 2.0f * BounceAmount, 1.0f, Ease.Linear);
        Action.Delay(temp, 0.350f);
        Action.Call(temp, ActionBounce);
    }

    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
        
        if (Bounce)
        {
            transform.position = new Vector3(transform.position.x, StartingPostion.y + HeightOffset, transform.position.z);
        }

    }
}
