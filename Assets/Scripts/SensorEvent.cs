using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorSample {

	public string _dateTimeString;
	public string _valueString;
	public SensorKnownEvents _event;

	// Constructor
	public SensorSample(string dateTimeString, string valueString)
	{
		_dateTimeString = dateTimeString;
		_valueString = valueString;

		//TODO: If desired, can put logic to include the enum here.
		// But, that really doesn't have to happen on every frame
	}

}
