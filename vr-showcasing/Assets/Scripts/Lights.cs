using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Håndterer opprettelse av lys
//Festes til en skripholder

//[RequireComponent(typeof(Light))]
public class Lights : MonoBehaviour {

	//skal ha Referanse til spillobjekter
    public GameObject controller; //skal ha referanse til kontroll
    public GameObject lightKeeper; //skal ha referanse til skriptholder
    public GameObject prefab; //skal ha referanse til lys-prefab
	
    public static GameObject activeLightObject = null; //referanse til aktivt lys

	//skal ha referanser til sliders for egenskaper til lys
	//Sliders må ha referanse til sine korresponderende metoder
    public Slider intensitySlider;
    public Slider spotAngleSlider;

    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

	//skal ha referanser til Tekst som vil indikere verdier til egenskaper
    public Text intensityText;
    public Text spotAngleText;
    public Text rangeText;

	//Verdier til egenskaper
    float intensity = 1;
    float spotAngle = 30;
    float range = 10;

    bool lightInHand = false; //for om et lys blir holdt

    bool currentlySpot = false; //hvis lystypen er spot

	//for om det skal festes til modell
    public Toggle toggle; //skal ha referanse til toggle
    bool attachToModelBool = false;

    Vector2 ranging; //for å justere rekkevidde med venstre kontroll
    float rangeGrowth = 1f;

    FindModels findModel; //Finner aktiv modell ved hjelp av denne

    // Use this for initialization
    void Start () {
		
		//setter verdier til tekst
        intensityText.text = intensity.ToString("#.##");
        spotAngleText.text = spotAngle.ToString("#.##");
        rangeText.text = range.ToString("#.##");

		//trenger FindModels komponent for å ta i bruk skriptet
        findModel = this.gameObject.AddComponent<FindModels>();
    }

	//hvis skriptholderen slås på
    private void OnEnable()
    {
		//finner alle spillobjekter tagget som "Lights"
        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");
		
		//Slår på mesh og gripe-skript på alle lys
        foreach (GameObject go in lights)
        {
            go.GetComponent<MeshRenderer>().enabled = true;
            go.GetComponent<GrabLightOrb>().enabled = true;
        }
    }

	//hvis skriptholderen slås av
    private void OnDisable()
    {
		//finner alle spillobjekter tagget som "Lights"
        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");

		//Slår av mesh og gripe-skript på alle lys
        foreach (GameObject go in lights)
        {
            go.GetComponent<MeshRenderer>().enabled = false;
            go.GetComponent<GrabLightOrb>().enabled = false;
        }
    }

    // Update is called once per frame
	//For å plassere ut lys og å endre rekkevidde på lys
    void Update ()
    {
		if(activeLightObject != null) //hvis det finnes et aktiv lys
        {
			//Hvis det aktive lyset er festet til kontroll
            if ((activeLightObject.transform.parent.GetInstanceID() == controller.transform.GetInstanceID()))            
            {
				//Hvis bruker slipper "B"-knappen på høyre kontroll
				if(OVRInput.GetUp(OVRInput.Button.Two))
                { 
					placeLight();
					lightInHand = false;
				}   
            }

			//Henter utslag på joystick til venstre kontroll
            ranging = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            if (ranging.x != 0) //Hvis det er utslag
            {
				
                if(ranging.x > 0 || range > 0) //Hvis rekkevidden skal økes eller er mer enn 0
                {
                    range = range + rangeGrowth * ranging.x * Time.deltaTime;
                }
                
                if (ranging.magnitude > 0.95f) //Hvis utslag er omtrent maks
                {
					//gjør endringen i rekkevidde raskere
                    rangeGrowth = rangeGrowth + 4 * Time.deltaTime;
                }

				//sett tekst for ny verdi for rekkevidde
                rangeText.text = range.ToString("#.##");

                Light lightComponent = activeLightObject.GetComponent<Light>();
                lightComponent.range = range;
            }
            
            else //Hvis det ikke er utslag på joystick
            {
                rangeGrowth = 1; //resett til 1
            }

			//Hvis bruker slipper "Y"-knappen på venstre kontroll
            if(OVRInput.GetDown(OVRInput.Button.Four))
            {
                destroyLight(); //Slett lyset fra scenen
            }
        }
	}

	//Oppretter nytt lys
    public void createNewLight()
    {
        if(lightInHand == false && GrabLightOrb.isGrabbed == false) //Hvis det ikke finnes et lys i hånden
        {
            GameObject newLight = Instantiate(prefab); //instansier kopi av lys-prefab
            Light lightComponent = newLight.GetComponent<Light>(); //henter referanse til lys-komponenten i lyset

            if (currentlySpot == true) //Hvis lys-type er satt til å være spot
            {
                lightComponent.type = LightType.Spot;
                lightComponent.spotAngle = spotAngle;
            }

			//Fester lyset til kontrollen
            newLight.transform.SetParent(controller.transform);
            newLight.transform.position = controller.transform.position;
            newLight.transform.rotation = controller.transform.rotation;
            activeLightObject = newLight;

            //Setter verdier til egenskaper
            lightComponent.intensity = intensity;
            lightComponent.range = range;
            activeLightObject.GetComponent<Light>().color = new Color(redSlider.value / 255, greenSlider.value / 255, blueSlider.value / 255, 100);

            lightInHand = true;
            GrabLightOrb.isGrabbed = true;
        } 
    }

	//For utplassering av lys
    public void placeLight()
    {
        if (attachToModelBool == false) //Hvis det ikke skal festes til modell
        {
            activeLightObject.transform.SetParent(lightKeeper.transform, true);
        }

        else
        {
            if (ModelBrowser.currentActiveModel != "")
            {
                activeLightObject.transform.SetParent(findModel.passModel(ModelBrowser.currentActiveModel).transform, true);
            }
        }

        GrabLightOrb.isGrabbed = false;
    }

	//Setter lys-type til å være point
    public void setPointLight()
    {
        if(currentlySpot == true && activeLightObject != null)
        {
            Light lightComponent = activeLightObject.GetComponent<Light>();
            lightComponent.type = LightType.Point;
            currentlySpot = false;
        }   
    }

	//Setter lys-type til å være spot
    public void setSpotLight()
    {
        if (currentlySpot == false && activeLightObject != null)
        {
            Light lightComponent = activeLightObject.GetComponent<Light>();
            lightComponent.type = LightType.Spot;
            lightComponent.spotAngle = spotAngle;
            currentlySpot = true;
        }
    }

	//sletter lyset fra scenen
    public void destroyLight()
    {
        if(activeLightObject != null)
        {
            Destroy(activeLightObject);
            lightInHand = false;
            GrabLightOrb.isGrabbed = false;
            activeLightObject = null;
        }  
    }

	//Endrer intensiteten til lyset ved endring på slider
    public void setIntensity()
    {
        if(activeLightObject != null)
        {
            Light lightComponent = activeLightObject.GetComponent<Light>();
            lightComponent.intensity = intensity;
            intensity = intensitySlider.value;
            intensityText.text = intensity.ToString("#.##");
        }     
    }

	//setter spot-vinkel til lyset ved endring på slider hvis lystype er spot
    public void setSpotAngle()
    {
        if (activeLightObject != null)
        {
            Light lightComponent = activeLightObject.GetComponent<Light>();
            lightComponent.spotAngle = spotAngle;
            spotAngle = spotAngleSlider.value;
            spotAngleText.text = spotAngle.ToString("#.##");
        }
    }

	//Setter lysfarge ved endring på farge-sliders
    public void setColorValue()
    {
        if(activeLightObject != null)
        {
            activeLightObject.GetComponent<Light>().color = new Color(redSlider.value / 255, greenSlider.value / 255, blueSlider.value / 255, 1);
        }   
    }

	//Sletter ALLE lys fra scenen
    public void removeAllLights()
    {
        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");

        foreach(GameObject go in lights)
        {
            Destroy(go);
        }

        GrabLightOrb.isGrabbed = false;
        lightInHand = false;
        activeLightObject = null;
    }

	//Henter den boolske verdien fra toggle som bestemmer om lys skal festes til modell eller ikke
    public void attachToModel()
    {
        attachToModelBool = toggle.GetComponent<Toggle>().isOn;
    }
}