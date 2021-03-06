﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BufferEvent : UnityEvent<int> { }

public class SampleBuffer : MonoBehaviour
{

	public static SampleBuffer instance;
	public List<SensorSample> _buffer;
	public BufferEvent _onBufferUpdated;
	[HideInInspector]
	public int _readFrom = 0;

	void Awake ()
	{

		if (instance == null) {
			instance = this;
		}

		// Initialize buffer
		_buffer = new List<SensorSample> ();

		// Initialize buffer event
		if(_onBufferUpdated == null)
			_onBufferUpdated = new BufferEvent();

	}

	void Start ()
	{
		// Listen to the SensorListener for new samples
		SensorListener.instance._onNewSamples.AddListener(ReadSamplesIntoBuffer);
	}

	public void ReadSamplesIntoBuffer (SensorSample[] samples)
	{

		// Are there already samples in the buffer?
		// If not, skip this loop and just add them to the buffer.
		if (_buffer.Count > 0) {

			// Starting with the first sample, loop through
			// to see if that sample appears anywhere in the buffer
			for (int i = 0; i < samples.Length; i++) {

				// Does this sample already exist in the buffer?
				// (we're using dateTimeString as a match code)
				Predicate<SensorSample> sampleFinder = (SensorSample s) => { return (s._dateTimeString == samples[i]._dateTimeString); };
				bool exists = _buffer.Exists(sampleFinder);

				// If so, continue and check the next sample.
				if (exists) {
					
					continue;

				// If not, stop here and add all subsequent samples to the buffer.
				} else {
					
					// Add the new samples to the buffer
					SensorSample[] newSamples = new SensorSample[samples.Length - i];
					Array.Copy(samples, i, newSamples, 0, samples.Length - i);
					AddSamplesToEndOfBuffer (newSamples);

					break;
				}
			}
				
		} else {

			// Debug.Log ("Buffer is empty. Adding samples.");
			// Add them to the list
			AddSamplesToEndOfBuffer (samples);

		}

		Debug.Log ("Buffer Count: " + _buffer.Count);

	}

	private void AddSamplesToEndOfBuffer (SensorSample[] samples)
	{

		// Capture the 'most recent' sample index from the buffer
		_readFrom = _buffer.Count;

		// Add them to the list
		foreach (SensorSample sample in samples) {
			_buffer.Add (sample);
		}

		// Notify any watchers that the buffer has been updated.
		// Debug.Log ("BufferUpdated invoked. Should read buffer from " + _readFrom);
		if (_onBufferUpdated != null) {
			_onBufferUpdated.Invoke (_readFrom);
		}

	}

}
