﻿using System.Collections;
using System.Collections.Generic;
using System;


using UnityEngine;
using UnityEngine.Networking;

using SimpleJSON;

public class SensorListener : MonoBehaviour {

	public static SensorListener instance;

	string _clientId = "T8rV9nTSuqeGzkYXsE5uwqbCnlxijPh39sETNEgq";
	string _clientSecret = "uO2S7sL7fHp5GupB2iIguDXoQKjO3jwb0VRW0lliW5El9xuUCY";
	string _sensorId = "0f43d3ad-0d5d-4616-9497-d86808ab727f";
	string _csHost = "https://bd-test.andrew.cmu.edu:81";
	string _dsHost = "https://bd-test.andrew.cmu.edu:82";
	private string _accessToken;
	private bool _poll = false;

	public string[] _eventsToDetect = {"knocking","microwave","door"};
	public float _refreshHz = 10;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		
		StartCoroutine(GetAccessToken());

	}
	
	// Update is called once per frame
	void Update ()
	{


	}
		
	private IEnumerator GetAccessToken()
	{
		string uri = String.Format("{0}/oauth/access_token/client_id={1}/client_secret={2}",_csHost,_clientId,_clientSecret);
		UnityWebRequest req = UnityWebRequest.Get (uri);
		yield return req.SendWebRequest ();

		string result = req.downloadHandler.text;
		Debug.Log (result);

		var N = ParseJson (result);

		// Assign Access Token
		_accessToken = N ["access_token"];

		_poll = true;
		StartCoroutine (PollForSensorData ());

		yield return null;			
	}

	private IEnumerator PollForSensorData()
	{

		// r = requests.get('%s/api/sensor/%s/timeseries?start_time=%d&end_time=%d' % (ds_host, sensor_id, timestamp_start, timestamp_now), headers=headers)

		string timestampStart = "0";
		string timestampEnd = "1524103712";
		
		string uri = String.Format("{0}/api/sensor/{1}/timeseries?start_time={2}&end_time={3}", _dsHost, _sensorId, timestampStart, timestampEnd);

		while (_poll) {
			

			Debug.Log ("Poll for Sensor Data Called");

			Debug.Log ("Polling: " + uri);
			UnityWebRequest req = UnityWebRequest.Get (uri);
			req.SetRequestHeader ("Authorization", "Bearer " + _accessToken);
			req.SetRequestHeader ("Accept", "application/json");

			yield return req.SendWebRequest ();

			string result = req.downloadHandler.text;
			Debug.Log (result);

			var N = ParseJson (result);

			JSONArray jsonSensorSamples = N["data"]["series"][0]["values"].AsArray;
			SensorSample[] sensorSamples = ParseJsonArray (jsonSensorSamples);
			Debug.Log (sensorSamples);

			// Send to the Sensor Broadcaster
			SensorEventBroadcaster.instance.OnSensorEventsChanged (sensorSamples);

			yield return new WaitForSeconds (1.0f / _refreshHz);
		}

		yield return null;

	}

	// parse result
	private JSONNode ParseJson(string data)
	{
		// Debug.Log ("Response:");
		// Debug.Log(data);
		// var N = SimpleJSON.JSON.Parse("[" + data + "]");
		var N = SimpleJSON.JSON.Parse(data);

		return N;
	}

	private SensorSample[] ParseJsonArray(JSONArray jsonArray)
	{
		
		SensorSample[] sensorSamples = new SensorSample[jsonArray.Count];

		for (int i = 0; i < sensorSamples.Length; i++)
		{
			sensorSamples [i] = new SensorSample(jsonArray[0].Value,jsonArray[1].Value);
		}

		return sensorSamples;

	}


}

