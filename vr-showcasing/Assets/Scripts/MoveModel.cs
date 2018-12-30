using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Forflytter modeller ved utslag på joysticker
//Må festes til en skriptholder

public class MoveModel : MonoBehaviour
{
    public static GameObject activeGameObject = null; //for referanse til modell som skal forflyttes
	
	//for verdier for utslag på joysticker
    Vector2 movementXZ;
    Vector2 movementY;

	//Må ha referanser til UI-elementer til tilhørende kanvas, sliders må ha referanse til dette skriptet
    public Text speedValue;
    public Text accelerationValue;
    public Slider speedSlider;
    public Slider accelerationSlider;

	//Verdier for i hvilken grad forflytning skjer
    public float moveSpeed;
    public float acceleration;

    FindModels findModel;

    // Use this for initialization
    void Start()
    {
        acceleration = 2.0f;
        moveSpeed = 1;
		
		//trenger FindModels komponent for å ta i bruk FindModels-skriptet
        findModel = this.gameObject.AddComponent<FindModels>();
    }

	//når skriptholder aktiveres hentes aktiv 3D-modell for forflytning
    private void OnEnable()
    {
        activeGameObject = ModelBrowser.currentActiveGameObject;
    }

	//Setter hastighet for forflytning ved endring på slider
    public void setSpeed()
    {
        moveSpeed = speedSlider.value;
        speedValue.text = moveSpeed.ToString(); //setter ny verdi til indikator-tekst
    }

	//Setter akselerasjon for forflytning ved endring på slider
    public void setAcceleration()
    {
        acceleration = accelerationSlider.value;
        accelerationValue.text = acceleration.ToString(); //setter ny verdi til indikator-tekst
    }

	//Setter posisjonen til 3D-modellen tilbake til origo
    public void ResetPosition()
    {
        activeGameObject.transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Henter verdiene fra joystickene
        movementXZ = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        movementY = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        //Hvis det er et aktivt spillobjekt for manipulasjon
        if(activeGameObject != null)
        {
            //Hvis utslag på venstre joysticks y-akse; forflytt i lengde
            if (movementXZ.y != 0)
            {
                activeGameObject.transform.Translate
                    (activeGameObject.transform.forward * movementXZ.y * moveSpeed * Time.deltaTime);
            }

            //Hvis utslag på venstre joysticks x-akse; forflytt i bredde
            if (movementXZ.x != 0)
            {
                activeGameObject.transform.Translate
                    (activeGameObject.transform.right * movementXZ.x * moveSpeed * Time.deltaTime);
            }

            //Hvis utslag på høyre joysticks y-akse; forflytt i høyde
            if (movementY.y != 0)
            {
                activeGameObject.transform.Translate
                    (activeGameObject.transform.up * movementY.y * moveSpeed * Time.deltaTime);
            }

            //hvis utslag på venstre eller høyre joystick er over 0.95f; øk moveSpeed
            if ((movementY.magnitude > 0.95f) || (movementXZ.magnitude > 0.95f))
            {
                moveSpeed = moveSpeed + acceleration * Time.deltaTime;
            }

            //Hvis ingen utslag på verken joystick; reset moveSpeed
            if (movementXZ.magnitude == 0 && movementY.magnitude == 0)
            {
                moveSpeed = 1;
            }
        }
    }
}