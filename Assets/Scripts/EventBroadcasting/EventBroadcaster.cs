using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBroadcaster : Singleton<EventBroadcaster>
{
    public delegate void ObserverAction(Parameters param = null);

    private Dictionary<string, ObserverAction> observers = new Dictionary<string, ObserverAction>();

    public void AddObserver(string eventName, ObserverAction observerAction)
    {
        if (this.observers.ContainsKey(eventName))
            this.observers[eventName] += observerAction;
        else
            this.observers.Add(eventName, observerAction);
    }
    
    public void RemoveObserver(string eventName)
    {
        if (this.observers.ContainsKey(eventName))
        {
            this.observers[eventName] = null;
            this.observers.Remove(eventName);
        }
    }

    public void RemoveObserverAtAction(string eventName, ObserverAction action)
    {
        if (this.observers.ContainsKey(eventName))
            this.observers[eventName] -= action;
    }

    public void PostEvent(string eventName, Parameters param = null)
    {
        if (this.observers.ContainsKey(eventName))
        {
            ObserverAction action = this.observers[eventName];
            action?.Invoke(param);
        }
    }
}
