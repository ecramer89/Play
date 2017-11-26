using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EventQueue: MonoBehaviour  {

	private IEnumerator<Action> dispatch;
	private Queue<ISerializedEvent> events=new Queue<ISerializedEvent>();

	private static EventQueue instance;
	public static EventQueue Instance{
		get {
			return instance;

		}
	}


	public void EnqueueEvent(ISerializedEvent evt){
		events.Enqueue(evt);

	}

	public void Awake(){
		instance = this;
        GameObject.DontDestroyOnLoad(gameObject);
	}


	public void Update(){

		if(dispatch != null && dispatch.MoveNext()){
			do{
				dispatch.Current();
			}while(dispatch.MoveNext());

			return;
		}

		if(events.Count > 0){
			dispatch = events.Dequeue().Handlers().GetEnumerator();
		}

	}


}






