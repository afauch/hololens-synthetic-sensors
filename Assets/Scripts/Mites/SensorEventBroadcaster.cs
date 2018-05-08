using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SensorEvent : UnityEvent {}

public enum SensorManagedEvents
{
	knocking,
	microwave,
	door
}

/// <summary>
/// This class translates from raw events detected by the Buffer Reader,
/// into managed events known by the Unity application.
/// </summary>
public class SensorEventBroadcaster : MonoBehaviour {

	public static SensorEventBroadcaster instance;

	private string _lastEvents;

	// All sensor events
	public SensorEvent knockingEvent;
	public SensorEvent microwaveEvent;
	public SensorEvent boardEvent;

	// Strings to read for
	const string knockingString = "Knocking";
	const string microwaveString = "Microwave Running";
	const string boardString = "Writing on Boarding";

	void Awake() {

		if (instance == null) {
			instance = this;
		}
	}

	void Start () {

		// Subscribe to BufferReader
		BufferReader.instance._onEventDetected.AddListener(BroadcastHandler);

	}

	public void BroadcastHandler(string sensorEvent) {

		sensorEvent = sensorEvent;

		switch (sensorEvent) {
		case knockingString:
			Debug.Log ("Calling event for " + sensorEvent);
			knockingEvent.Invoke();
			break;
		case microwaveString:
			Debug.Log ("Calling event for " + sensorEvent);
			microwaveEvent.Invoke();
			break;
		case boardString:
			Debug.Log ("Calling event for " + sensorEvent);
			boardEvent.Invoke();
			break;
		default:
			break;
		}

	}

}
