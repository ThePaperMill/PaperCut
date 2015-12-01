/****************************************************************************/
/*!
\file   ItemTextBox.cs
\author Steven Gallwas
\brief  
    Controls the item text box.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;
using ActionSystem;

public class ItemTextBox : EventHandler
{
  private ActionGroup Seq = new ActionGroup();
  public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
  private TextMesh SpriteText;
  public Vector3 LerpPosition = new Vector3(0.0f, -2.0f, 0.0f);

    // Use this for initialization
  void Start () 
  {
    EventSystem.GlobalHandler.Connect(Events.ActivateSelector, OnActivateSelector);
    EventSystem.GlobalHandler.Connect(Events.DeactivateSelector, OnDeactivateSelector);
    EventSystem.GlobalHandler.Connect(Events.UpdateItemText, OnUpdateItemText);
    SpriteText = gameObject.transform.FindChild("SpriteText").gameObject.GetComponent<TextMesh>();
  }

  void OnUpdateItemText(EventData data)
  {
        UpdateItemTextEvent temp = (UpdateItemTextEvent)data;

        SpriteText.text = temp.NewText;
  }

  void OnActivateSelector(EventData data)
  {
    // lerp down to the camera
    var test = ActionSystem.Action.Sequence(Seq);
    LerpPosition.z = transform.localPosition.z;

    Action.Property(test, gameObject.transform.GetProperty(o => o.localPosition), LerpPosition, 1.25, Curve);
  }

  void OnDeactivateSelector(EventData data)
  {
    // lerp up away from the camera
    var test = ActionSystem.Action.Sequence(Seq);
    var finalPos = new Vector3(0.0f, 5.0f, 0.0f);
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
    EventSystem.GlobalHandler.Disconnect(Events.ActivateSelector, OnActivateSelector);
    EventSystem.GlobalHandler.Disconnect(Events.DeactivateSelector, OnDeactivateSelector);
    EventSystem.GlobalHandler.Disconnect(Events.UpdateItemText, OnUpdateItemText);
  }
}
