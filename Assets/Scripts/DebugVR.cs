using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugVR : MonoBehaviour {

    public static DebugVR instance;

    private TextMesh textMesh;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {

        textMesh = this.gameObject.GetComponent<TextMesh>();

	}

    public void Log(string text)
    {
        textMesh.text = text;
    }

}
