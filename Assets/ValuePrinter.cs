using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Prints raw values from BluetoothWatcher to 3D Text objects
public class ValuePrinter : MonoBehaviour {

    public TextMesh _values;

	// Use this for initialization
	void Start () {

        // BETTER WAY - subscribe to the ValueReceived method
        // TODO: Figure out how to make this threadsafe
        // BluetoothWatcher.instance.ValueReceived += OnValueReceived;

	}

    // BETTER WAY - subscribe this method to the BluetoothWatcher script
    // TODO: figure out how to make this threadsafe
    //void OnValueReceived(string value)
    //{
    //    _values.text = value;
    //}

    private void Update()
    {
        _values.text = BluetoothWatcher.instance.lastValueReceived;
    }

}
