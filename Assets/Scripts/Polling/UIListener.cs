using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crayon;

// TODO: This is a temp class for 4/16 that should be discarded
public class UIListener : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnEvent(string thisEvent) {

		Debug.Log (thisEvent + " event heard by UIListener");
		this.gameObject.SetState (CrayonStateType.Custom, "activated");

	}
	

}
