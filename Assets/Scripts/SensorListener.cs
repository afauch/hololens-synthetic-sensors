using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using SimpleJSON;

public class ListenerEvent : UnityEvent<SensorSample[]> {}

public class SensorListener : MonoBehaviour {

	public static SensorListener instance;

	[Header("BuildingDepot")]
	public string _clientId = "HoR418ntNCbAYQA7R6j8rqdNexMehYOcSNqIFesT";
	public string _clientSecret = "PfAvtUbFlQPH9ZqKDBgg2NcXtSlHwwPGja8rDrpRQcnj2TUnhE";
	public string _sensorId = "ab411355-e6b1-483f-b4b2-7f3e0dee361c";
	string _csHost = "https://bd-test.andrew.cmu.edu:81";
	string _dsHost = "https://bd-test.andrew.cmu.edu:82";

	private string _accessToken;
	private bool _poll = false;

	[Header("Proto Host")]
	public bool _useProtoHost = false;
	string _protoHost = "http://localhost:8080/response.json";

	[Header("Events")]
	public float _refreshHz = 1;
	public ListenerEvent _onNewSamples;

	void Awake()
	{
		if (instance == null) {
			instance = this;
		}

		if(_onNewSamples == null)
			_onNewSamples = new ListenerEvent();

	}

	// Use this for initialization
	void Start ()
	{

		if (_useProtoHost) {
			_poll = true;
			StartCoroutine (PollForSensorData ());
		} else {
			StartCoroutine (GetAccessToken ());
		}

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
		// Debug.Log (result);

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

		// it accepts timestamp
		string timestampStart = "0";
		// string timestampEnd = "1524103712";
		Int32 timestampEnd = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

		string uri;
		if (_useProtoHost) {
			uri = _protoHost;
		} else {
			uri = String.Format("{0}/api/sensor/{1}/timeseries?start_time={2}&end_time={3}", _dsHost, _sensorId, timestampStart, timestampEnd.ToString());
		}
			
		while (_poll) {
			

			// Debug.Log ("Poll for Sensor Data Called");

			Debug.Log ("Polling: " + uri);
			UnityWebRequest req = UnityWebRequest.Get (uri);

			// Only include these if we're doing the real thing.
			if (!_useProtoHost) {
				req.SetRequestHeader ("Authorization", "Bearer " + _accessToken);
				req.SetRequestHeader ("Accept", "application/json");
			}

			yield return req.SendWebRequest ();

			string result = req.downloadHandler.text;
			Debug.Log (result);

			var N = ParseJson (result);

			JSONArray jsonSensorSamples = N["data"]["series"][0]["values"].AsArray;
			SensorSample[] sensorSamples = ParseJsonArray (jsonSensorSamples);

			// Send to the Sensor broadcaster
			// SampleBuffer.instance.ReadSamplesIntoBuffer (sensorSamples);
			if (_onNewSamples != null) {
				_onNewSamples.Invoke (sensorSamples);
			}

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
			sensorSamples [i] = new SensorSample(jsonArray[i][0].Value,jsonArray[i][2].Value);
		}
		return sensorSamples;

	}


}

