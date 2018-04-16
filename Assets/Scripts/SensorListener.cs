using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.Networking;

using SimpleJSON;

public class SensorListener : MonoBehaviour {

	public static SensorListener instance;

	public string _urlBase = "http://localhost:8080/response.json";
	public string[] _eventsToDetect = {"knocking","microwave","door"};
	public float _refreshRate;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		StartCoroutine(SendRequest(_urlBase));

	}
		
	private IEnumerator SendRequest(string requestString)
	{
		// Make the request
		UnityWebRequest request = UnityWebRequest.Get(requestString);
		yield return request.Send();

		string result = request.downloadHandler.text;

		// call function to Parse JSON
		ParseJson(result);

		yield return null;

	}

	// parse result
	private void ParseJson(string data)
	{
		Debug.Log ("Response:");
		Debug.Log(data);
		// var N = SimpleJSON.JSON.Parse("[" + data + "]");
		var N = SimpleJSON.JSON.Parse(data);

		string sensorEvents = N["data"]["series"][0]["values"][0][0];
		Debug.Log (sensorEvents);

		// Send to the Sensor Broadcaster
		SensorEventBroadcaster.instance.OnSensorEventsChanged (sensorEvents);

	}


}

