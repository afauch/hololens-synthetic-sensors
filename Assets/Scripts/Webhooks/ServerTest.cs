using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using UnityEngine;

#if UNITY_EDITOR
using System.Net;
using System.Net.Sockets;
#endif

public class ServerTest : MonoBehaviour
{

	// TODO: See this article on how to retrofit the sockets
	// protocol into something that will work for HoloLens
	// https://foxypanda.me/tcp-client-in-a-uwp-unity-app-on-hololens/

	TcpListener server;
	public int _port = 12345;
	bool _run = true;

	int _debugCounter = 0;

	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.S)) {
			StartServer (_port);
		}
			
		if (Input.GetKeyUp (KeyCode.X)) {
			StopServer ();
		}

		if (Input.GetKeyUp (KeyCode.T)) {
			StartCoroutine (TestLoop());
		}
	}

	public void StartServer (int port)
	{
		_run = true;
		UnityEngine.Debug.Log ("StartServer called");
		IPAddress address = IPAddress.Parse ("127.0.0.1");
		server = new System.Net.Sockets.TcpListener (address, _port);
		server.Start ();

		StartCoroutine (Listen ());

	}

	public void StopServer ()
	{
		_run = false;
	}

	IEnumerator TestLoop ()
	{
		while (true == true) {
			yield return new WaitForSeconds (1.0f);
			UnityEngine.Debug.Log ("while test working");
		}
	}


	IEnumerator Listen ()
	{

		// Buffer for reading data
		byte[] bytes = new byte[1024];
		string data;

		//Enter the listening loop
		while (_run) {

			UnityEngine.Debug.Log (_debugCounter);

			Console.Write ("Waiting for a connection... ");

			// Perform a blocking call to accept requests.
			// You could also user server.AcceptSocket() here.
			TcpClient client = server.AcceptTcpClient ();
			Console.WriteLine ("Connected!");

			// Get a stream object for reading and writing
			NetworkStream stream = client.GetStream ();

			int i;

			// Loop to receive all the data sent by the client.
			i = stream.Read (bytes, 0, bytes.Length);

			while (i != 0) {
				// Translate data bytes to a ASCII string.
				data = System.Text.Encoding.ASCII.GetString (bytes, 0, i);
				Console.WriteLine (String.Format ("Received: {0}", data));

				// Process the data sent by the client.
				data = data.ToUpper ();

				byte[] msg = System.Text.Encoding.ASCII.GetBytes (data);

				// Send back a response.
				stream.Write (msg, 0, msg.Length);
				Console.WriteLine (String.Format ("Sent: {0}", data));

				i = stream.Read (bytes, 0, bytes.Length);
				yield return new WaitForSeconds(1.0f);

			}

			// Shutdown and end connection
			client.Close ();

			_debugCounter++;
			yield return new WaitForSeconds(1.0f);

		}

		yield return new WaitForSeconds(1.0f);

	}

}