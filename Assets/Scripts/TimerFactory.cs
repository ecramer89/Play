using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerFactory : MonoBehaviour {

    [SerializeField] GameObject timerPrefab;
    private static TimerFactory instance;

    public static TimerFactory Instance
    {
        get
        {

            return instance;
        }
    }

    public void Awake()
    {
        instance = this;
    }


    public Timer NewTimer()
    {
        return Instantiate(timerPrefab).GetComponent<Timer>();
    }


}
