using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Unity Event base that others can subscribe to
/// </summary>
[System.Serializable]
public class SensorEvent : UnityEvent
{
}

public enum SensorKnownEvents
{
	knocking,
	microwave,
	door
}

public class SensorEventBroadcaster : MonoBehaviour {

	public static SensorEventBroadcaster instance;

	private string _lastEvents;

	// All sensor events
	public SensorEvent knockingEvent;
	public SensorEvent microwaveEvent;
	public SensorEvent doorEvent;

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnSensorEventsChanged(SensorSample[] samples){

		Debug.Log (samples);

		// Reverse the array so it runs from 

		// Check which events are new
		string newEvent = CompareEvents(samples);

		// Debug.Log ("newEvent = " + newEvent + "; " + "lastEvent = " + _lastEvents);
		// BroadcastHandler (newEvent);




	}

	private string CompareEvents(SensorSample[] samples){

		// We only care about samples we haven't seen yet.

		return null;
	}

	private void BroadcastHandler(string eventsList)
	{

		switch (eventsList) {
		case "knocking":
			knockingEvent.Invoke();
			break;
		case "microwave":
			microwaveEvent.Invoke();
			break;
		case "door":
			doorEvent.Invoke();
			break;
		default:
			break;
		}

	}

}
