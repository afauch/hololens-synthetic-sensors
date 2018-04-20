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
	public SensorEvent doorEvent;

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

		Debug.Log ("Calling event for " + sensorEvent);

		sensorEvent = sensorEvent.ToLower ();

		switch (sensorEvent) {
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
