using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ISerializedEvent  {

	IEnumerable<Action> Handlers();
	string Name();
    void UnSubscribe(ISubscriber subsciber);
}

public class ParameterlessEvent : ISerializedEvent
{
	private Dictionary<ISubscriber, Action> subscribers = new Dictionary<ISubscriber, Action>();
	private string name;

	public ParameterlessEvent(string name){
		this.name = name;
	}

	public string Name(){
		return name;
	}

    public void UnSubscribe(ISubscriber subscriber)
    {
        if (!subscribers.ContainsKey(subscriber)) return;
        subscribers.Remove(subscriber);
    }

    public void Subscribe(ISubscriber subscriber, Action handler){
		subscribers[subscriber] = handler;
	}

	public void Fire(){
		EventQueue.Instance.EnqueueEvent(this);
	}

	public IEnumerable<Action> Handlers(){
		return subscribers.Values;
	}

}

public class UnaryParameterizedEvent<T> : ISerializedEvent
{
	private Dictionary<ISubscriber, Action<T>> typedSubscribers;
	private List<Action> generifiedSubscribers;
	string name;

	public UnaryParameterizedEvent(string name){
		this.name = name;
		typedSubscribers = new Dictionary<ISubscriber, Action<T>>();
	}

	public string Name(){
		return name;
	}

	public void Subscribe(ISubscriber subscriber, Action<T> handler){
        typedSubscribers[subscriber] = handler;  

	}

    public void UnSubscribe(ISubscriber subscriber)
    {
        if (!typedSubscribers.ContainsKey(subscriber)) return;
        typedSubscribers.Remove(subscriber);
    }

	public void Fire(T arg0){
        Debug.Log("firing event: " + name);
        generifiedSubscribers = new List<Action>();
		foreach(Action<T> handler in typedSubscribers.Values){
			generifiedSubscribers.Add(()=>handler(arg0));
		}
		EventQueue.Instance.EnqueueEvent(this);
	}

	public IEnumerable<Action> Handlers(){
		return generifiedSubscribers;
	}

}

public class BinaryParameterizedEvent<T,V> : ISerializedEvent
{
	private Dictionary<ISubscriber, Action<T,V>> typedSubscribers;
	private List<Action> generifiedSubscribers;
	private string name;


	public BinaryParameterizedEvent(string name){
		typedSubscribers = new Dictionary<ISubscriber, Action<T, V>>();
		this.name = name;
	}

	public string Name(){
		return name;
	}

	public void Subscribe(ISubscriber subscriber, Action<T,V> handler){
        typedSubscribers[subscriber] = handler;
 
	}


    public void UnSubscribe(ISubscriber subscriber)
    {
        if (!typedSubscribers.ContainsKey(subscriber)) return;
        typedSubscribers.Remove(subscriber);
    }

    public void Fire(T arg0, V arg1){
        Debug.Log("firing event: " + name);
        generifiedSubscribers = new List<Action>();
		foreach(Action<T,V> subscriber in typedSubscribers.Values){
			generifiedSubscribers.Add(()=>subscriber(arg0, arg1));
		}
		EventQueue.Instance.EnqueueEvent(this);
	}

	public IEnumerable<Action> Handlers(){
		return generifiedSubscribers;
	}

}
	
 
