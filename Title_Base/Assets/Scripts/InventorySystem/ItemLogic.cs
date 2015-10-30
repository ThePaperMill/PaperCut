using UnityEngine;
using System.Collections;
using ActionSystem;

public class ItemLogic : EventHandler
{
  private ActionSequence Seq = new ActionSequence();
  public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);


  public ItemLogic()
  {
    EventSystem.GlobalHandler.Connect(Events.MoveItem, OnMoveItem);
    EventSystem.GlobalHandler.Connect(Events.ActivateSelector, OnActivateSelector);
  }

  // Use this for initialization
  void Start ()
  {

  }

  void OnMoveItem(EventData data)
  {
    var ActionS = Action.Sequence(Seq);

    // cast as the correct type
    MoveItemEvent temp = (MoveItemEvent)data;

    // store our current position
    Vector3 CurPos = gameObject.transform.position;

    // lerp by the given amount
    Action.Property(ActionS, gameObject.transform.GetProperty(o => o.localPosition), CurPos + temp.MoveAmount, 0.25, Curve);
  }

  void OnActivateSelector(EventData data)
  {
    // lerp down to the camera.
    var test = ActionSystem.Action.Sequence(Seq);
    var finalPos = new Vector3(10.0f, 10.0f, 10.0f);
    finalPos.z = transform.localPosition.z;
    Action.Property(test, gameObject.transform.GetProperty(o => o.localPosition), finalPos, 1.5, Curve);
  }


  // Update is called once per frame
  void Update ()
  {
    Seq.Update(Time.deltaTime);
	}

  void OnDestroy()
  {
    EventSystem.GlobalHandler.Disconnect(Events.MoveItem, OnMoveItem);
    EventSystem.GlobalHandler.Disconnect(Events.ActivateSelector, OnActivateSelector);
  }
}
