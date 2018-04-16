using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crayon;

public class KnockingListener : MonoBehaviour {

	// Use this for initialization
	void Start () {

		// Subscribe to knocking event
		SensorEventBroadcaster.instance.knockingEvent.AddListener(OnKnocking);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnKnocking ()
	{
		Debug.Log ("Knocking event heard by KnockingListener");
		this.gameObject.SetState (CrayonStateType.Selected);

	}
}
