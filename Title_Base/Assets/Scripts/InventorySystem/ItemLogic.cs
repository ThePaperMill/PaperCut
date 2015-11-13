using UnityEngine;
using System.Collections;
using ActionSystem;

public class ItemLogic : EventHandler
{
  private ActionGroup Seq = new ActionGroup();
  public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);


  public ItemLogic()
  {
    EventSystem.GlobalHandler.Connect(Events.MoveItem, OnMoveItem);
    //EventSystem.GlobalHandler.Connect(Events.ActivateSelector, OnActivateSelector);
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
    Vector3 CurPos = gameObject.transform.localPosition;

    // lerp by the given amount
    Action.Property(ActionS, gameObject.transform.GetProperty(o => o.localPosition), CurPos + temp.MoveAmount, 0.25, Curve);
  }

  void OnActivateSelector(EventData data)
  {
    // lerp down to the camera.
    var test = ActionSystem.Action.Sequence(Seq);
    var finalPos = new Vector3(0.0f, -1.0f, 0.0f);
    finalPos.z = transform.localPosition.z;
    Action.Property(test, gameObject.transform.GetProperty(o => o.localPosition), transform.localPosition + finalPos, 1.5, Curve);
  }


  // Update is called once per frame
  void Update ()
  {
    Seq.Update(Time.deltaTime);
    //print(transform.localPosition);
	}

  void OnDestroy()
  {
    EventSystem.GlobalHandler.Disconnect(Events.MoveItem, OnMoveItem);
    EventSystem.GlobalHandler.Disconnect(Events.ActivateSelector, OnActivateSelector);
  }
}
