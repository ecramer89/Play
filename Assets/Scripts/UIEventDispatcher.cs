using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//single class that dispatches UI events that multiple classes need to know about, and which can have variadic (generic) parameters.
public class UIEventDispatcher : MonoBehaviour {

    private static UIEventDispatcher instance;
    public static UIEventDispatcher Instance
    {
        get
        {
            return instance;
        }
    }

 
    public event Action<string> PlayerEnteredNewInput = (string input)=> { Debug.Log(String.Format("Fired: PlayerEnteredNewInput: {0}", input)); };

    public void FirePlayerEnteredNewInput(String newInput)
    {
        PlayerEnteredNewInput(newInput);

    }

    public void Awake()
    {
        instance = this;
    }

}
