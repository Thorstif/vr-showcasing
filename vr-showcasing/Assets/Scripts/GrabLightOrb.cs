using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For å la bruker flytte og skaffe referanse til utplasserte lys ved å "gripe" dem
//Skal være inkludert på lys-prefab

public class GrabLightOrb : MonoBehaviour {

    bool isGrabable = false; //bestemmer om lyset kan gripes
    public static bool isGrabbed = false; //for om lys blir holdt
    GameObject collidedWith = null; //spillobjektet som collider treffer
    Renderer rend; // for å ha synlig mesh
	
	private void Start()
    {
		//henter referanse til komponenten for synlighet
        rend = GetComponent<Renderer>();       
    }

    // Update is called once per frame
    void Update ()
    {
        if(isGrabbed == false) //hvis et lys blir holdt
        {
            if (isGrabable == true) //hvis lyset kan gripes 
            {
                if (OVRInput.GetDown(OVRInput.Button.Two)) //hvis "B"-knappen på høyre kontroll trykkes
                {
                    isGrabbed = true;
					
					//fester lyset til høyre kontroll
                    this.transform.SetParent(collidedWith.transform.parent.transform, true);
					
					//setter det aktive lyset i Lights-skriptet til dette lyset
                    Lights.activeLightObject = this.gameObject;
                }
            }
        }      
	}

	//Skjer ved kollisjon av annen collider, krever en collider-komponent
    private void OnTriggerEnter(Collider other)
    {
        if(isGrabbed == false) //hvis et annet lys ikke allerede er holdt
        {
			isGrabable = true; //setter det til gripbart
			collidedWith = other.gameObject; //lager referanse til kontrollen
            GetComponent<Renderer>().material.color = Color.green; //gjør mesh grønt
        }
    }

	//Skjer når colliders går fra hverandre
    private void OnTriggerExit(Collider other)
    {
        isGrabable = false; //vil ikke lenger være gripbar
        GetComponent<Renderer>().material.color = new Color(0, 1, 0, 39 / 255); //gir meshet tilbake originalfargen
    }
}