#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class WebhooksManager : MonoBehaviour {

	public static WebhooksManager instance;

	public int _port = 9999;
	public string _ip;
	public string _absoluteUri = "";

	[Header("Device")]
	public string _clientDeviceId; // this is a device ID to accompany the endpoint
	public string _fullClientEndpoint;	// this is the HoloLens client endpoint that will receive updates; this gets sent to the _sensorServerEndpoint

	public string _sensorServerEndpoint; // this is the URL for the webhooks server that will be doing polling for us then posting to our _fullClientEndpoint

	void Awake() {
		if (instance == null) {
			instance = this;
		}
	}

	// Use this for initialization
	void Start () {
		_fullClientEndpoint = ConstructEndpoint ();
	}

	/// <summary>
	/// Constructs the endpoint to send to the webhooks server.
	/// </summary>
	/// <returns>The endpoint.</returns>
	private string ConstructEndpoint() {
		_ip = GetLocalIPAddress();
		return String.Format ("http://{0}:{1}{2}", _ip, _port, _absoluteUri);
	}

	// From here: https://stackoverflow.com/questions/6803073/get-local-ip-address
	public static string GetLocalIPAddress()
	{
		var host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (var ip in host.AddressList)
		{
			if (ip.AddressFamily == AddressFamily.InterNetwork)
			{
				Debug.Log (ip.ToString ());
				return ip.ToString();
			}
		}
		throw new Exception("No network adapters with an IPv4 address in the system!");
	}

	// Method for handling
	public void SubscribeToSensor(VirtualSensor sensor)
	{
		StartCoroutine (SubscribeToSensor (sensor._sensorServerEndpoint));
	}

	// Post the client endpoint to the sensor server
	private IEnumerator SubscribeToSensor(string uri)
	{
		string reqBody = "{\"device_id\":\"" + _clientDeviceId + "\", \"endpoint\":\"" + _fullClientEndpoint + "\"}";
		UnityWebRequest req = UnityWebRequest.Post (uri, reqBody);
		req.SetRequestHeader ("Content-Type", "application/json");
		Debug.Log ("Sending request: " + reqBody);
		yield return req.Send ();

		string result = req.downloadHandler.text;
		Debug.Log (result);

		yield return null;			
	}


}
#endif