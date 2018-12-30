using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Lytter etter trykk på knapp for venstre pekefinger
//roterer bruker umiddelbart 45 grader mot klokken

public class Rotate45OnButton : MonoBehaviour {
	
    // Update is called once per frame
	void Update ()
    {
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            transform.rotation = Quaternion.Euler(0, -45, 0) * transform.rotation;
        }     
    }
}

