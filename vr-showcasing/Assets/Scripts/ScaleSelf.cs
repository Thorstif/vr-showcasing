using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//skalerer brukeren i det virtuelle miljøet ved utslag på venstre joystick
//Må festet til en skriptholder

public class ScaleSelf : MonoBehaviour
{
    public GameObject self; //Må ha referanse til VR-bruker-oppsett
	
    public Text currentScale; //Tekst for å indikere skala til bruker

	//Verdier for skala-endring
    public float growth;
    public float acceleration;
    float growthspeed;
	
    Vector2 scaling; //verdi for utslag på joystick

    // Use this for initialization
    void Start()
    {
        currentScale.text = "1";
        growth = 1f;
        growthspeed = growth;
        acceleration = 2.0f;
    }

	//Setter skala tilbake til original
    public void resetScale()
    {
        self.transform.localScale = new Vector3(1,1,1);
        currentScale.text = "1";
    }

    // Update is called once per frame
    void Update()
    {
		//Henter verdi fra joystick
        scaling = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        if (scaling.x != 0)
        {
            growthspeed = growth * scaling.x * Time.deltaTime;

            self.transform.localScale = new Vector3(self.transform.localScale.x + growthspeed,
                                                    self.transform.localScale.y + growthspeed,
                                                    self.transform.localScale.z + growthspeed);

			//hvis utslag på joystick er over 0.95f; øk hastighet for skala-endring
            if (scaling.magnitude > 0.95f)
            {
                growth = growth + acceleration * Time.deltaTime;
            }

            currentScale.text = self.transform.localScale.y.ToString(); //endrer tekst til å vise ny skala
        }

		//Hvis ingen utslag på joystick; reset hastighet for skala-endring
        else
        {
            growth = 1;
        }
    }
}
