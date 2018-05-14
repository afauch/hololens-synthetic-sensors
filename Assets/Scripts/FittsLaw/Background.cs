using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class Background : MonoBehaviour, IFocusable, IInputHandler {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnFocusEnter()
    {
        Debug.Log("Cursor Entered " + gameObject.name);
    }

    public void OnFocusExit()
    {
        Debug.Log("Cursor Exited " + gameObject.name);
    }

    public void OnInputDown(InputEventData eventData)
    {
        Debug.Log("Click down " + gameObject.name);
    }

    public void OnInputUp(InputEventData eventData)
    {
        Debug.Log("Click up " + gameObject.name);
    }

}
