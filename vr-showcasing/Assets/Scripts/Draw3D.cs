/**************************************************
 * Draw3D class allows the user to draw lines in the air
 * by using the right touch controller
 * Uses Linerendrer in Unity and a table
 * with 2 or more points in 3D-space and draws a 
 * line between them.
***************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draw3D : MonoBehaviour {
    //global variables used throughout the code
    private GameObject model;
    public GameObject rHand,capsule;
    public Slider slider;
    public Text ModelName;
    LineRenderer lRend;
    Stack<GameObject> gameObjects = new Stack<GameObject>();
    int linePoints = 0;
    Material mat;
    Color color;

    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

    /** During start the drawing tool color is set to white with a thickness of 0.01**/
    void Start () {

        color = Color.white;
        slider.value = 0.01f;
        model = gameObject;
        
    }
	
	void Update () {
		//indikasjon på hvor tegningen kommer "ut" fra, er ikke aktivt før brukeren
        //har aktivert tegne verktøyet og har pekefingeren nærme pekefingeravtrekkeren
        capsule.SetActive(false);
		//når fingeren er nærme pekefingeravtrekkeren så aktiveres tegne verktøy indikasjonen
        if(OVRInput.Get(OVRInput.NearTouch.SecondaryIndexTrigger)==true)
            capsule.SetActive(true);
		//første gang pekefingeravtrekker trykkes inn 
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            //Opretter nytt spillobjekt
            GameObject gOb = new GameObject();
			//setter objektet i stakken som kan senere brukes til å fjerne tegningen
            gameObjects.Push(gOb);
			//spillobjektet knytter seg til valgte modellen fra modelbrowser klassen
            gOb.transform.parent = model.transform;
            //legger til linerenderer som komponent til spill objektet
            //linerenderer tillater en å tegne linjer i lufta basert 
            //på punkter i 3D-rommet
            lRend = gOb.AddComponent<LineRenderer>();
			//setter tykkelse på tegnigne basert på slideren fra tegneverktøy menyen
            lRend.endWidth = slider.value;
            lRend.startWidth = slider.value;
			//opretter nytt material
            mat = new Material(Shader.Find("Self-Illumin/Diffuse"));
			//setter farge på materialet til tegningen basert på 
            //farge som ble valgt i tegneverktøyet
            mat.SetColor("_Color", color);
			//tildeler materialet til linerenderer
            lRend.material = mat;
			//denne må være med for at tegneverktøyet skal kunne skalerers med modellene
            lRend.useWorldSpace = false;
            linePoints = 0;

        }
		//mens pekefingeravtrekkeren holdes inne
        else if(OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            //øker tabellstørrelsen på linerenderer tabellen som tillater for flere punkter
            lRend.positionCount = linePoints + 1;
			//hver punkt får verdi basert på posisjonen til høyre hånd kontroller
            lRend.SetPosition(linePoints, rHand.transform.position);
			//øker så lenge knappen holdes inne og kontrolleren beveges
            linePoints++;

        }   
		//Langfingeravtrekkeren brukes for å slette siste sammensatte tegningen i stakken
        //kan slette alle så lenge en trykker langfingeravtrekkeren flere ganger
        else if(OVRInput.GetDown((OVRInput.Button.SecondaryHandTrigger)))
        {
            if (gameObjects.Count > 0)
            {
                Destroy(gameObjects.Pop());
            }          
        }
	}
	/**Metode for å knytte tegningen til valgt 3D-modell**/
    public void attachment()
    {
        model = ModelBrowser.currentActiveGameObject;
    }
    /**Rød farge for tegningen **/
    public void redColor(){
        color = Color.red;    
    }
    /**Blå farge for tegningen **/
    public void blueColor(){
        color = Color.blue;  
    }
    /**Svart farge for tegningen **/
    public void blackColor(){
        color = Color.black;   
    }
    /**Grønn farge for tegningen **/
    public void greenColor(){
        color = Color.green;    
    }
    /**Gul farge for tegningen **/
    public void yellowColor(){
        color = Color.yellow; 
    }
    /**Hvit farge for tegningen **/
    public void whiteColor(){
        color = Color.white; 
    }
    /**Egendefinert farge for tegningen **/
    public void setColorValue()
    {
        color = new Color(redSlider.value / 255, greenSlider.value / 255, blueSlider.value / 255, 1);
    }
}