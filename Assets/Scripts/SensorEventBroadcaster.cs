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

	public void OnSensorEventsChanged(string eventsList){

		// TODO: This needs to be changed to a loop to handle an array of events

		// Check which events are new
		string newEvent = CompareEvents(eventsList);

		Debug.Log ("newEvent = " + newEvent + "; " + "lastEvent = " + _lastEvents);
		BroadcastHandler (newEvent);


	}

	private string CompareEvents(string eventsList){

		// TODO: This needs to be modified to handle an array of events

		if (eventsList == _lastEvents) {
			_lastEvents = eventsList;
			return null;
		} else {
			_lastEvents = eventsList;
			return eventsList;
		}

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
		case "doorEvent":
			doorEvent.Invoke();
			break;
		default:
			Debug.Log ("Unrecognized event: " + eventsList);
			break;
		}

	}

}
