using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Roterer modeller ved utslag på joysticker
//Må festes til en skriptholder

public class RotateModel : MonoBehaviour
{
	
    public static GameObject activeGameObject; //for referanse til modell som skal forflyttes
    
	//for om rotasjon skjer i forhold til seg selv eller worldspace
	public GameObject toggle;
	public bool worldSpace = false;
	
	//for verdier for utslag på joysticker
    Vector2 rotationXZ;
    Vector2 rotationY;


    FindModels findModel;

    // Use this for initialization
    void Start()
    {
		//trenger FindModels komponent for å ta i bruk FindModels-skriptet
        findModel = this.gameObject.AddComponent<FindModels>();
    }

	//når skriptholder aktiveres hentes aktiv 3D-modell for rotering
    private void OnEnable()
    {
        activeGameObject = ModelBrowser.currentActiveGameObject;
    }

	//UI-knapper trenger referanse til disse metodene
	//roterer modell 45 grader i de forskjellige aksene
	//var originalt 90, men endret til 45 senere (derfor metodenavnene)
    public void rotateX90()
    {
        if (worldSpace == false)
        {
            activeGameObject.transform.Rotate(Vector3.right * 45);
        }

        else
        {
            activeGameObject.transform.Rotate(Vector3.right * 45, Space.World);
        }
    }

    public void rotateZ90()
    {
        if (worldSpace == false)
        {
            activeGameObject.transform.Rotate(Vector3.forward * 45);
        }

        else
        {
            activeGameObject.transform.Rotate(Vector3.forward * 45, Space.World);
        }
    }

    public void rotateY90()
    {
        if (worldSpace == false)
        {
            activeGameObject.transform.Rotate(Vector3.up * 45);
        }

        else
        {
            activeGameObject.transform.Rotate(Vector3.up * 45, Space.World);
        }
    }

	//resetter rotasjon
    public void resetRotation()
    {
        activeGameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

	//bestemmer om rotasjon skjer ifølge med modellen eller world space
    public void setIfWorldSpace()
    {
        worldSpace = toggle.GetComponent<Toggle>().isOn; //henter verdien fra toggle som boolean
        //print("WorldSpace=" + worldSpace); //for debugging
    }

    // Update is called once per frame
    void Update()
    {
		//Henter verdiene fra joystickene
        rotationXZ = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        rotationY = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

		//Hvis utslag på venstre joysticks x-akse; roter roll
        if (rotationXZ.x != 0)
        {
            if(worldSpace == false)
            {
                activeGameObject.transform.Rotate(Vector3.right * Time.deltaTime * rotationXZ.x * 20);
            }

            else
            {
                activeGameObject.transform.Rotate(Vector3.right * Time.deltaTime * rotationXZ.x * 20, Space.World);
            }
            
        }

		//Hvis utslag på venstre joysticks y-akse; roter pitch
        if (rotationXZ.y != 0)
        {
            if (worldSpace == false)
            {
                activeGameObject.transform.Rotate(Vector3.forward * Time.deltaTime * rotationXZ.y * 20);
            }

            else
            {
                activeGameObject.transform.Rotate(Vector3.forward * Time.deltaTime * rotationXZ.y * 20, Space.World);
            }
        }

		//Hvis utslag på høyre joysticks x-akse; roter yaw
        if (rotationY.x != 0)
        {
            if (worldSpace == false)
            {
                activeGameObject.transform.Rotate(Vector3.up * Time.deltaTime * rotationY.x * 20);
            }

            else
            {
                activeGameObject.transform.Rotate(Vector3.up * Time.deltaTime * rotationY.x * 20, Space.World);
            }
        }
    }
}
