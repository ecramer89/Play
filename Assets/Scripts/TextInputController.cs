﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInputController : MonoBehaviour {

    [SerializeField] Text playerInput;

    public void BroadcastPlayerInput()
    {
        MyEvents.PlayerEnteredNewInput.Fire(playerInput.text);
    }
}
