/****************************************************************************/
/*!
\file   ScientistRequest.cs
\author Steven Gallwas
\brief  
       The request conversation for the scientist.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ConversationSystem
{
  public class ScientistRequest : ConversationAction
  {
    public GameObject Scientist = null;
    private ScientistReqEvent SciReq = null;
    private BoolEvent BoolEventData = new BoolEvent();

    public ConversationAction NextIfTrue;
    public ConversationAction NextIfFalse;
    public ConversationAction NextIfNone;

    public override void Start()
    {
      SciReq = new ScientistReqEvent(Scientist);

      base.Start();
      if (NextIfTrue == null)
      {
        var next = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("TalkAction")).AddComponent<TalkAction>();
        //next.Text = "Thanks for the \n<b>" + ItemName + "</b>!";
        NextIfTrue = next;
      }
      if (NextIfFalse == null)
      {
        var next = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("TalkAction")).AddComponent<TalkAction>();
        //next.Text = "This isn't an \n<b>" + ItemName + "</b>...";
        NextIfFalse = next;
      }
      if (NextIfNone == null)
      {
        var next = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("TalkAction")).AddComponent<TalkAction>();
        //next.Text = "Come back when you have \na <b>" + ItemName + "</b>.";
        NextIfNone = next;
      }

    }

    public override void StartAction()
    {
      // Request an item from the player
      EventSystem.GlobalHandler.DispatchEvent(Events.ScientistReq, SciReq);

      EventSystem.GlobalHandler.Connect(Events.RecievedItem, OnRecievedItem);
    }

    void OnRecievedItem(EventData eventData)
    {
      //If we recieved the proper item.
      var data = eventData as RecievedItemEvent;
      
      //if the item is null
      if (data == null || data.Info.ItemName == "")
      {
        Next = NextIfNone;
      }

      else
      {
        if (data.Info.Transformable == true)
        {
          BoolEventData.IsTrue = true;
          Next = NextIfTrue;

          // dispatch a transformation event with the item to transform.
          RecievedItemEvent iEvent = new RecievedItemEvent(data.Info);
          EventSystem.GlobalHandler.DispatchEvent(Events.TransformItem, iEvent);
        }
        else
        {
          BoolEventData.IsTrue = false;
          Next = NextIfFalse;
        }
      }


      this.Disconnect(Events.RecievedItem, OnRecievedItem);
      //Dispatching whether or not the correct item was recieved.
      EventSystem.GlobalHandler.DispatchEvent(Events.RecievedProperItem, BoolEventData);

      this.DispatchEvent(Events.NextAction);
    }

    // Update is called once per frame
    void Update()
    {

    }
  }
}

