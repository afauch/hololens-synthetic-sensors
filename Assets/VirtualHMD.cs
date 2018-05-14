using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualHMD : MonoBehaviour {


	// As a proxy for 'listening for Bluetooth advertisements,'
	// instead look for Zone collisions in 3D space
	void OnTriggerEnter(Collider other){

		Debug.Log (this.gameObject.name + " entered " + other.gameObject.name);


	}

	void OnTriggerExit(Collider other) {

		Debug.Log (this.gameObject.name + " exited " + other.gameObject.name);

	}

}
