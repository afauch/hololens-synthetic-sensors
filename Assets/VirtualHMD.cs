using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualHMD : MonoBehaviour {

	WebhooksManager _whm;

	void Start() 
	{
		_whm = GetComponent<WebhooksManager> ();
	}

	// As a proxy for 'listening for Bluetooth advertisements,'
	// instead look for Zone collisions in 3D space
	void OnTriggerEnter(Collider other){

		Debug.Log (this.gameObject.name + " entered " + other.gameObject.name);
		VirtualSensor sensor = other.gameObject.GetComponent<VirtualSensor> ();
		if (sensor != null) {
			_whm.SubscribeToSensor (sensor);
		}


	}

	void OnTriggerExit(Collider other) {

		Debug.Log (this.gameObject.name + " exited " + other.gameObject.name);
		VirtualSensor sensor = other.gameObject.GetComponent<VirtualSensor> ();
		if (sensor != null) {
			_whm.UnsubscribeFromSensor(sensor);
		}

	}

}
