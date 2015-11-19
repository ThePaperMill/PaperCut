using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public static class EventSystem
{
    public static char GlobalHandler;
    public static EventData DefaultEvent = new EventData();

    //The GameObject is the listener, 
    private static Dictionary<object, Dictionary<String, List<Action<EventData>>>> EventList = new Dictionary<object, Dictionary<String, List<Action<EventData>>>>();

    

    static public void EventConnect(object listener, String eventName, Action<EventData> func)
    {
        if (!EventList.ContainsKey(listener))
        {
            EventList.Add(listener, new Dictionary<String, List<Action<EventData>>>());
        }

        var listeningObj = EventList[listener];
        if (!listeningObj.ContainsKey(eventName))
        {
            listeningObj.Add(eventName, new List<Action<EventData>>());
        }


        listeningObj[eventName].Add(func);

    }

    static public void EventDisconnect(object target, String eventName, object thisPointer = null)
    {
        if (thisPointer == null)
        {
            thisPointer = target;
        }
        if (!EventList.ContainsKey(target))
        {
            return;
        }

        var listeningObj = EventList[target];
        if (!listeningObj.ContainsKey(eventName))
        {
            return;
        }
        var functionList = listeningObj[eventName];
        for (int i = 0; i < functionList.Count(); ++i)
        {
            if (functionList[i].Target == thisPointer)
            {
                functionList.RemoveAt(i);
                break;
            }
        }
        if (functionList.Count() == 0)
        {
            EventList.Remove(eventName);
        }
        if (listeningObj.Count() == 0)
        {
            EventList.Remove(listeningObj);
        }
    }

    static public void EventDisconnect(object target, String eventName, Action<EventData> function)
    {

        if (!EventList.ContainsKey(target))
        {
            return;
        }

        var listeningObj = EventList[target];
        if (!listeningObj.ContainsKey(eventName))
        {
            return;
        }
        var functionList = listeningObj[eventName];
        for (int i = 0; i < functionList.Count(); ++i)
        {
            if (functionList[i] == function)
            {
                functionList.RemoveAt(i);
                break;
            }
        }
        if (functionList.Count() == 0)
        {
            EventList.Remove(eventName);
        }
        if (listeningObj.Count() == 0)
        {
            EventList.Remove(listeningObj);
        }
    }

    static public void EventSend(object target, String eventName, EventData eventData = null)
    {
        if (!EventList.ContainsKey(target))
        {
            return;
        }
        var listeningObj = EventList[target];
        if (!listeningObj.ContainsKey(eventName))
        {
            return;
        }
        if (eventData == null)
        {
            eventData = DefaultEvent;
        }
        var functionList = listeningObj[eventName];
        
        //if(eventName == "NextActionEvent")
        //{
        //  foreach(var whatever in functionList)
        //    Debug.Log(whatever.Target);
        //}


        for (var i = 0; i < functionList.Count(); ++i)
        {
            var func = functionList[i];

            if (func.Target == null || func.Target.Equals(null))
            {
              Debug.Log("wat");
              //continue;
            }
            else
              Debug.Log(func.Target);

            if (func != null)
            {
                func(eventData);
            }
            else
            {
                functionList.RemoveAt(i);
                --i;
            }
            
        }
    }

    public static void DisconnectObject(object listener)
    {
        if(EventList.ContainsKey(listener))
        {
            EventList.Remove(listener);
        }
    }

    //ExtensionMethods
    public static void DispatchEvent<TObject>(this TObject instance, String eventName, EventData eventData = null)
    {
      EventSystem.EventSend(instance, eventName, eventData);
    }
    public static void Connect<TObject>(this TObject instance, String eventName, Action<EventData> function)
    {
        EventSystem.EventConnect(instance, eventName, function);
    }
    public static void Disconnect<TObject>(this TObject instance, String eventName, Action<EventData> function)
    {
        EventSystem.EventDisconnect(instance, eventName, function);
    }
    public static void Disconnect<TObject>(this TObject instance, String eventName, object funcThisPointer)
    {
        EventSystem.EventDisconnect(instance, eventName, funcThisPointer);
    }
}

public class EventHandler : MonoBehaviour
{
    void OnDestroy()
    {
        EventSystem.DisconnectObject(this);
        EventSystem.DisconnectObject(this.gameObject);
    }
}

public class EventData
{
    
}



