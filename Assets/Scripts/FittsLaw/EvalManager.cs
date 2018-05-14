using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvalManager : MonoBehaviour {

    public static EvalManager _instance;
    public GameObject _cursor;

    void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        // Debug.Log(_cursor.transform.position);
    }

}
