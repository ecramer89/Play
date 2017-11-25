using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class StoryEvent  {

    string description;
    private HashSet<Action> subscribers;
    public StoryEvent(string description)
    {
        this.description = description;
    }


    public void Subscribe(Action action)
    {
        subscribers.Add(action);
    }

    public void Unsubscribe(Action action)
    {
        subscribers.Remove(action);
    }

    public void Fire()
    {
        Debug.Log(String.Format("Story event {0}:", description));
        foreach(Action action in subscribers){
            action();
        }
    }
}
