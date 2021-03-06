﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BufferReaderEvent : UnityEvent<string> {}

public class BufferReader : MonoBehaviour {

	public static BufferReader instance;

	private int _hasReadThrough;
	private string _lastEvent = null;

	public BufferReaderEvent _onEventDetected;

	void Awake() 
	{
		if (instance == null) {
			instance = this;
		}

		if (_onEventDetected == null)
			_onEventDetected = new BufferReaderEvent ();

	}

	void Start () {

		if (_onEventDetected != null)
			_onEventDetected = new BufferReaderEvent ();

		// Subscribe to SampleBuffer events
		SampleBuffer.instance._onBufferUpdated.AddListener(ReadSamplesFromBuffer);

	}

	void ReadSamplesFromBuffer(int readFrom)
	{

		// Read through the buffer, sample by sample, looking for a 'START:' event
		for (int i = readFrom; i < SampleBuffer.instance._buffer.Count; i++) {

			string sampleEvent = SampleBuffer.instance._buffer [i]._valueString;

			// If it's not the same as the last event, that means it's a new event.
			if (sampleEvent.StartsWith ("START:")) {
				// trim the "START" off
				sampleEvent = sampleEvent.Substring (6, sampleEvent.Length - 6);
				Debug.Log (sampleEvent);
				if (_onEventDetected != null) {
					_onEventDetected.Invoke (sampleEvent);
				}
			}

			// Update the global counter
			_hasReadThrough = i;

		}

	}

}
