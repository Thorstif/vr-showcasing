using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For det virtuelle meny-systemet
//Viser eller skjuler meny-systemet basert på trykk eller slipp av venstre gripeknapp

public class TouchMenuSystem : MonoBehaviour {
	// Update is called once per frame
	void Update () {

        //Henter fram menykanvasene ved trykk av venstre gripeknapp
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            this.gameObject.transform.Find("Canvases").gameObject.SetActive(true);
        }

        //Deaktiverer menykanvasene ved slipp av venstre gripeknapp
        if(OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
        {
            this.gameObject.transform.Find("Canvases").gameObject.SetActive(false);
        }
    }
}
