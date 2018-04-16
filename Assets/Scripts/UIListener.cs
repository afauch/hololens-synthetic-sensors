using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crayon;

// TODO: This is a temp class for 4/16 that should be discarded
public class UIListener : MonoBehaviour {

	public SensorKnownEvents _eventToHear;

	// Use this for initialization
	void Start () {

		// Subscribe to the event based on 
		switch (_eventToHear) {
		case SensorKnownEvents.Knocking:
			SensorEventBroadcaster.instance.knockingEvent.AddListener (OnEvent);
			break;
		case SensorKnownEvents.Microwave:
			SensorEventBroadcaster.instance.microwaveEvent.AddListener (OnEvent);
			break;
		case SensorKnownEvents.Door:
			SensorEventBroadcaster.instance.doorEvent.AddListener (OnEvent);
			break;
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEvent() {

		Debug.Log (_eventToHear.ToString () + " heard by UIListener");
		this.gameObject.SetState (CrayonStateType.Custom, "activated");

	}
	

}
