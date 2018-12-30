using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Håndterer veksling mellom menyene i det virtuelle meny-systemet

public class VirtualButtonScripts : MonoBehaviour
{
    public bool menuIsUp;
	
	//for referanse til alle canvasene
    GameObject mainCanvas, enviCanvas, fileCanvas, menxCanvas, modlCanvas, moveCanvas, scalCanvas, rotaCanvas, sslfCanvas, mslfCanvas, ligtCanvas, collCanvas, coldCanvas, drawCanvas, quikCanvas;
    
	//skal ha referanse til skriptholdere som blir aktivert og deaktivert mellom menyer
	public GameObject moveModelScriptKeeper;
    public GameObject scaleModelScriptKeeper;
    public GameObject rotateModelScriptKeeper;
    public GameObject scaleSelfScriptKeeper;
    public GameObject lightScriptKeeper;
    public GameObject moveSelfScriptKeeper;
    public GameObject quickSetupScripKeeper;


    bool fromDraw = false; //for hvis det byttes fra tegnemenyen

    public Text textObject; //må ha referanse til modell-indikator-tekst i modelutforsker-menyen

    // Use this for initialization
    void Start()
    {
		//henter referanse til alle kanvasene basert på navn
        modlCanvas = transform.Find("ModelBrowser").gameObject;
        rotaCanvas = transform.Find("ModelRotator").gameObject;
        enviCanvas = transform.Find("Environment").gameObject;
        scalCanvas = transform.Find("ModelScaler").gameObject;
        fileCanvas = transform.Find("FileBrowser").gameObject;
        menxCanvas = transform.Find("TouchMenuX").gameObject;
        moveCanvas = transform.Find("ModelMover").gameObject;
        sslfCanvas = transform.Find("SelfScaler").gameObject;
        mslfCanvas = transform.Find("SelfMover").gameObject;
        mainCanvas = transform.Find("MainMenu").gameObject;
        drawCanvas = transform.Find("DrawMenu").gameObject;
        collCanvas = transform.Find("ColorsForLights").gameObject;
        coldCanvas = transform.Find("ColorsForDrawing").gameObject;
        quikCanvas = transform.Find("QuickSetup").gameObject;
        ligtCanvas = transform.Find("Light").gameObject;
    }

    // Update is called once per frame
	/* 
    void Update()
    {
        //ShowMenu();
    }
	*/

    #region CANVAS HANDLING

    // Åpner kanvas for henting av objektfil og lukker kanvas for hovedmeny
    public void File()
    {
        mainCanvas.SetActive(false);
        fileCanvas.SetActive(true);
    }

	//Sletter valgt modell
    public void DeleteModel()
    {
        if (ModelBrowser.currentActiveGameObject != null)
        {

           // Destroy(ModelBrowser.currentActiveGameObject);

            //modlCanvas.GetComponent<ModelBrowser>().enabled = false;
            //modlCanvas.SetActive(false);

            //ModelBrowser.currentActiveModel = "";
            textObject.text = "";

            //modlCanvas.SetActive(true);
            //modlCanvas.GetComponent<ModelBrowser>().enabled = true; //PRØVER Å TRIGGER'E "onEnable" MEN DET FUNKER IKKE

        }
    }

	//returnerer fra lysfarge- til lys-meny
    public void returnFromLightColors()
    {
        ligtCanvas.SetActive(true);
        collCanvas.SetActive(false);
    }

	//fra lys- til lysfarge-meny
    public void fromLightToLightColors()
    {
        collCanvas.SetActive(true);
        ligtCanvas.SetActive(false);
    }

	//returnerer fra tegnefarge- til tegne-meny
    public void returnFromDrawColors()
    {
        drawCanvas.SetActive(true);
        coldCanvas.SetActive(false);
    }

	//fra tegne- til tegnefarge-meny
    public void fromDrawToDrawColors()
    {
        drawCanvas.SetActive(false);
        coldCanvas.SetActive(true);
    }

	//Veksler fra hovemeny til lys-meny
    public void Light()
    {
        ligtCanvas.SetActive(true);
        mainCanvas.SetActive(false);

        lightScriptKeeper.SetActive(true);
    }

	//Veksler fra hovemeny til selv-skalerings-meny
    public void ScaleSelf()
    {
        sslfCanvas.SetActive(true);
        mainCanvas.SetActive(false);

        scaleSelfScriptKeeper.SetActive(true);
    }

	//Veksler fra hovemeny til selv-forflytnings-meny
    public void MoveSelf()
    {
        mslfCanvas.SetActive(true);
        mainCanvas.SetActive(false);

        moveSelfScriptKeeper.SetActive(true);
    }

	//Veksler fra hovemeny til modell-forflytning-meny
    public void MoveModels()
    {
        if(ModelBrowser.currentActiveModel != "")
        {    
            moveCanvas.SetActive(true);
            mainCanvas.SetActive(false);

            moveModelScriptKeeper.SetActive(true);   
        }      
    }

	//Veksler fra hovemeny til modell-skalering-meny
    public void ScaleModels()
    {
        if (ModelBrowser.currentActiveModel != "")
        {
            scalCanvas.SetActive(true);
            mainCanvas.SetActive(false);

            scaleModelScriptKeeper.SetActive(true);                 
        }
    }

	//Veksler fra hovemeny til modell-rotering-meny
    public void RotateModels()
    {
        if (ModelBrowser.currentActiveModel != "")
        {
            rotaCanvas.SetActive(true);
            mainCanvas.SetActive(false);

            rotateModelScriptKeeper.SetActive(true);
        }
    }

    //Veksler fra hovemeny til modell-skalering-meny
    public void QuickTransform()
    {
        if (ModelBrowser.currentActiveModel != "")
        {
            quikCanvas.SetActive(true);
            mainCanvas.SetActive(false);

            quickSetupScripKeeper.SetActive(true);
        }
    }

    //Veksler fra hovemeny til modellutforsker-meny
    public void Models()
    {
        textObject.text = ModelBrowser.currentActiveModel;
        mainCanvas.SetActive(false);
        modlCanvas.SetActive(true);
    }

	//Veksler fra tegne- til modellutforsker-meny
    public void changeModelFromDraw()
    {
        drawCanvas.SetActive(false);
        modlCanvas.SetActive(true);
        fromDraw = true;
    }

	//Veksler fra referanse-oppsett- til hovedmeny
    public void LoadMenu()
    {
        menxCanvas.SetActive(false);
        mainCanvas.SetActive(true);
    }

	//Veksler fra hovemeny til miljø-meny
    public void ChangeEnvironmentMenu()
    {
        mainCanvas.SetActive(false);
        enviCanvas.SetActive(true);
        //Debug.Log("Change Env Menu");
    }

	//Veksler fra hovemeny til tegne-meny
    public void DrawMenu()
    {
        mainCanvas.SetActive(false);
        drawCanvas.SetActive(true);
    }

	//returnerer fra hvilkensomhelst meny til hovedmeny
    public void Return()
    {
        if(fromDraw == true)
        {
            drawCanvas.SetActive(true);
            modlCanvas.SetActive(false);
            fromDraw = false;
        }

        else
        {
			//deaktiverer skriptholdere
            rotateModelScriptKeeper.SetActive(false);
            scaleModelScriptKeeper.SetActive(false);
            moveModelScriptKeeper.SetActive(false);
            scaleSelfScriptKeeper.SetActive(false);
            moveSelfScriptKeeper.SetActive(false);
            lightScriptKeeper.SetActive(false);
            quickSetupScripKeeper.SetActive(false);

            //deaktiverer kanvaser
            enviCanvas.SetActive(false);
            fileCanvas.SetActive(false);
            modlCanvas.SetActive(false);
            menxCanvas.SetActive(false);
            moveCanvas.SetActive(false);
            scalCanvas.SetActive(false);
            rotaCanvas.SetActive(false);
            sslfCanvas.SetActive(false);
            mslfCanvas.SetActive(false);
            ligtCanvas.SetActive(false);
            drawCanvas.SetActive(false);
            quikCanvas.SetActive(false);

            //aktiverer hoved-meny-kanvas
            mainCanvas.SetActive(true);
        }
        
    }

    #endregion

	//for debugging
    public void Load()
    {
        //Debug.Log("Load Env");
    }

	//for test av start-knapp på touch-kontroll
    public void ShowMenu()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start) && enviCanvas.activeSelf == false)
        {
            if (menuIsUp)
            {
                mainCanvas.SetActive(false);
                menuIsUp = false;
            }
            else
            {
                mainCanvas.SetActive(true);
                menuIsUp = true;
            }
            //Debug.Log("Touch Menu Button Pressed");
        }

    }

	//avslutter programmet
    public void CloseProgram()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

	//for debugging
    public void Apply()
    {
        Return();
        Debug.Log("Apply");
    }

}
