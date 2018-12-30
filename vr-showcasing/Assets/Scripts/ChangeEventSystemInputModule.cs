using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Manages switching of VR and screen userinterfaces
//Can't have OVRInputModule and StandaloneInputModule active at the same time
//Must be attached to EventSystem which also has OVRInputModule and StandaloneInputModule

public class ChangeEventSystemInputModule : MonoBehaviour {

    private Component inputModuleVR, inputModulePC;
    public bool enablechanging = true; //control turning on and off 2D-menu
    public GameObject canvasController; //reference to skript-keeper which handles 2D menu

    // Use this for initialization
    void Start () 
	{
        //Fetches reference to scrip components
        //which are used for handling the userinterface in 2D and VR
        inputModuleVR = GetComponent<OVRInputModule>();
        inputModulePC = GetComponent<StandaloneInputModule>();
    }
	
	// Update is called once per frame
	void Update () 
	{
        changeInputModule();
    }
	
    //Listens for button presses from keyboard and touch controllers
    //Switches to correct InputModule depending of button activity
    public void changeInputModule()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Disables 2D menu and enables VR menu
            if (enablechanging == true)
            {
                GetComponent<OVRInputModule>().enabled = true;
                GetComponent<StandaloneInputModule>().enabled = false;
                enablechanging = false;
                canvasController.SetActive(false);
            }
			
            //Enables 2D menu and disables VR menu
            else
            {
                GetComponent<OVRInputModule>().enabled = false;
                GetComponent<StandaloneInputModule>().enabled = true;
                canvasController.SetActive(true);
                enablechanging = true;
            }
        }

        //Disables 2D menu and enables VR menu
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
		{
			GetComponent<OVRInputModule>().enabled = true;
			GetComponent<StandaloneInputModule>().enabled = false;
			canvasController.SetActive(false);
		}       
    }
}