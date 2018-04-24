using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebhooksTest : MonoBehaviour {

	public static WebhooksTest instance;

	public string _serverEndpoint = "http://localhost:8080/";
	public string _clientEndpoint = "xyz";

	void Awake() {

		if (instance == null) {
			instance = this;
		}

	}


	void Update () {

		if (Input.GetKeyUp (KeyCode.Space)) {

			StartCoroutine (PostURL ());

		}

	}

	private IEnumerator PostURL()
	{
		string uri = _serverEndpoint;
		string reqBody = "TEST-REQUEST";
		UnityWebRequest req = UnityWebRequest.Post (uri, reqBody);
		req.SetRequestHeader ("Content-Type", "text/plain");
		yield return req.SendWebRequest ();

		string result = req.downloadHandler.text;
		Debug.Log (result);

		yield return null;			
	}

}
