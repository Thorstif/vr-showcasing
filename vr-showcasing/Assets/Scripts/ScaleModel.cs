using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Skalerer modeller ved utslag på venstre joystick
//Må festes til en skriptholder

public class ScaleModel : MonoBehaviour
{
    public static GameObject activeGameObject = null; //for referanse til modell som skal forflyttes

	//Må ha referanser til UI-elementer til tilhørende kanvas, sliders må ha referanse til dette skriptet
    public Text growthValue;
    public Text accelerationValue;
    public Slider growthSlider;
    public Slider accelerationSlider;

	//Verdier for i hvilken grad forflytning skjer
    public float growth;
    public float acceleration;
    float growthspeed;
	
    Vector2 scaling; //verdi for utslag på joystick

    FindModels findModel;

    // Use this for initialization
    void Start()
    {
        growth = 1f;
        growthspeed = growth;
        acceleration = 2.0f;
        findModel = this.gameObject.AddComponent<FindModels>();
    }
	
	//når skriptholder aktiveres hentes aktiv 3D-modell for forflytning
    private void OnEnable()
    {
        activeGameObject = ModelBrowser.currentActiveGameObject;
    }

	//Setter hastighet for skala-endring ved endring på slider
    public void setGrowth()
    {
        growth = growthSlider.value;
        growthValue.text = growth.ToString("#.##");
    }

	//Setter akselerasjon for skala-endring ved endring på slider
    public void setAcceleration()
    {
        acceleration = accelerationSlider.value;
        accelerationValue.text = acceleration.ToString("#.##");
    }

	//resetter skala til original størrelse
    public void ResetScale()
    {
        if (activeGameObject != null)
        {
            activeGameObject.transform.localScale = new Vector3(
            (Mathf.Abs(activeGameObject.transform.localScale.x) / activeGameObject.transform.localScale.x),
            (Mathf.Abs(activeGameObject.transform.localScale.y) / activeGameObject.transform.localScale.y),
            (Mathf.Abs(activeGameObject.transform.localScale.z) / activeGameObject.transform.localScale.z));
        }    
    }

	//Inverterer 3-modellen om x-akse
    public void FlipX()
    {
        if (activeGameObject != null)
        {
            activeGameObject.transform.localScale = new Vector3(
                activeGameObject.transform.localScale.x * -1,
                activeGameObject.transform.localScale.y,
                activeGameObject.transform.localScale.z);
        }
    }

	//Inverterer 3-modellen om y-akse
    public void FlipY()
    {
        if (activeGameObject != null)
        {
            activeGameObject.transform.localScale = new Vector3(
                activeGameObject.transform.localScale.x, 
                activeGameObject.transform.localScale.y * -1, 
                activeGameObject.transform.localScale.z);
        }
    }

	//Inverterer 3-modellen om z-akse
    public void FlipZ()
    {
        if (activeGameObject != null)
        {
            activeGameObject.transform.localScale = new Vector3(
                activeGameObject.transform.localScale.x,
                activeGameObject.transform.localScale.y,
                activeGameObject.transform.localScale.z * -1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(activeGameObject != null && ObjectImporter.inProgress == false) //Hvis det er et aktivt spillobjekt for manipulasjon
        {
			//Henter verdiene fra joystick
            scaling = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            if (scaling.x != 0) //Hvis det er utslag på joystick
            {
                growthspeed = growth * scaling.x * Time.deltaTime;

				//endrer skala ved utslag på venstre joystick,
				// (x + (g * (abs(x) / x))) gir riktig selv om x er invertert
                activeGameObject.transform.localScale = new Vector3(
                    activeGameObject.transform.localScale.x + (growthspeed * Mathf.Abs(activeGameObject.transform.localScale.x) / activeGameObject.transform.localScale.x),
                    activeGameObject.transform.localScale.y + (growthspeed * Mathf.Abs(activeGameObject.transform.localScale.y) / activeGameObject.transform.localScale.y),
                    activeGameObject.transform.localScale.z + (growthspeed * Mathf.Abs(activeGameObject.transform.localScale.z) / activeGameObject.transform.localScale.z));

				//hvis utslag på joystick er over 0.95f; øk hastighet for skala-endring
                if (scaling.magnitude > 0.95f)
                {
                    growth = growth + acceleration * Time.deltaTime;
                }
            }

			//Hvis ingen utslag på joystick; reset hastighet for skala-endring
            else
            {
                growth = 1;
            }


            #region KEYBOARD TESTINPUT //test med keyboard
			/*
            if (Input.GetKey(KeyCode.W))
            {
                growthspeed = growth * Time.deltaTime;

                activeGameObject.transform.localScale = new Vector3(activeGameObject.transform.localScale.x + growthspeed, activeGameObject.transform.localScale.y + growthspeed, activeGameObject.transform.localScale.z + growthspeed);


                if (Input.GetKey(KeyCode.LeftShift))
                {
                    growth = growth + acceleration * Time.deltaTime;
                }
            }

            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
            {
                growth = 1;
            }

            if (activeGameObject.transform.localScale.x > 0)
            {
                if (Input.GetKey(KeyCode.S))
                {

                    growthspeed = growth * Time.deltaTime;

                    activeGameObject.transform.localScale = new Vector3(activeGameObject.transform.localScale.x - growthspeed, activeGameObject.transform.localScale.y - growthspeed, activeGameObject.transform.localScale.z - growthspeed);


                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        growth = growth + acceleration * Time.deltaTime;
                    }
                }
            }
			*/
            #endregion
        }
    }
}