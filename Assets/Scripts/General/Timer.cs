using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Timer : MonoBehaviour {

    public float secondsDuration;
    public Action onTimeUp;

    private float secondsElapsed = -1;
    
	void Update () {

        if (secondsElapsed >= 0) secondsElapsed += Time.deltaTime;
        if(secondsElapsed >= secondsDuration)
        {
            onTimeUp();
            Destroy(gameObject);
        }

    }

    public void Begin()
    {
        secondsElapsed = 0;
    }



}
