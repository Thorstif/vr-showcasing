using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleOutput : MonoBehaviour {
    private float axis1DValue;
	// Use this for initialization
	void Start () {
        print("Test");
	}
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.Get(OVRInput.Touch.Any))
            print("Button Y");


    }
}
