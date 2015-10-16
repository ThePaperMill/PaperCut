using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public static class EventSystem
{
    public static EventData DefaultEvent = new EventData();

    //The GameObject is the listener, 
    private static Dictionary<object, Dictionary<String, List<Action<EventData>>>> EventList = new Dictionary<object, Dictionary<String, List<Action<EventData>>>>();

    

    static public void EventConnect(object listener, String eventName, Action<EventData> func)
    {
        if(!EventList.ContainsKey(listener))
        {
            EventList.Add(listener, new Dictionary<String, List<Action<EventData>>>());
        }

        var listeningObj = EventList[listener];
        if(!listeningObj.ContainsKey(eventName))
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
        for(int i = 0; i < functionList.Count(); ++i)
        {
            if(functionList[i].Target == thisPointer)
            {
                functionList.RemoveAt(i);
                break;
            }
        }
        if(functionList.Count() == 0)
        {
            EventList.Remove(eventName);
        }
        if(listeningObj.Count() == 0)
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
        foreach(var i in functionList)
        {
            i(eventData);
        }
    }
}

public class EventData
{
    
}



