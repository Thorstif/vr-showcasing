/**************************************************
 * RayCaster klassen brukes for å la en bruker
 * bevege seg i VR-miljøet ved hjelp av teleportering
 * Ved bruke av Physics.Raycast får en koordinater
 * til punktet det pekes mot og bruker disse
 * koordinatene til å flytte kameraet til nytt
 * posisjon.
***************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public class RayCaster : MonoBehaviour
{
    //Lengden på racast strålen
    public float rayCastRange = 50f;
    public Transform rayCastStartPoint;
    //Sirkel som viser hvor brukeren teleporterer til
    public GameObject circle;  
    //Linerenderer brukes slikt for å se raycast strålen
    //siden den er usynlig ellers                                                                         
    private LineRenderer laserLine;
    //raycast strålen er bundet til høyre kontroller
    //og start punktet er også definert der
    private GameObject rightHand;
    //Så linerendere linjen får farge
    private Material material;
    private bool rotate;

    /**Initialiserer variabler**/
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        //finner høyrekontrolleren i hiarkeriet 
        rightHand = this.gameObject.transform.GetChild(0).GetChild(0).GetChild(5).GetChild(0).gameObject;
        //setter strålen usynlig ved start
        laserLine.enabled = false;
        //tildeler materialet og farge til strålen
        material = new Material(Shader.Find("Self-Illumin/Diffuse"));
        material.SetColor("_Color", Color.blue);
        laserLine.material = material;
    }

    /**Denne metoden kjøres hvert bilderamme (frame)**/
    void Update()
    {
       
        //Hvis pekefingeren ikke er nærme pekefingeravtrekkeren 
        if ((OVRInput.Get(OVRInput.NearTouch.SecondaryIndexTrigger)) == false)
        {
            //setter strålen synlig
            laserLine.enabled = true;
            //hente posisjonen til høyrehånd
            Vector3 rayOrigin = rightHand.transform.position;
            // Deklarer en hit punkt som brukes til å få tilbake melding som koordinater
            // og hva slags objekt som ble treffet av raycast
            RaycastHit hit;
            //startposisjonen til laser er ved pekefingeren
            laserLine.SetPosition(0, rayCastStartPoint.position);
            // Sjekker om raycast har treffet noe objekter som har en collider
            if (Physics.Raycast(rayOrigin, rightHand.transform.forward, out hit, rayCastRange))
            {
                //Sirkelen som indikerer hvor brukeren ender opp 
                //blir synlig
                circle.SetActive(true);
                //Størrelsen på sirkelen
                circle.transform.localScale = new Vector3(0.8f, 0.0001f, 0.8f);
                //sirkelen beveger seg med koordinater som fås fra raycast
                circle.transform.position = hit.point;
                //Sirkelen er kun synlig når det pekes mot gulvet, og ikke andre objekter
                if (hit.collider.tag != "Plane")
                {
                    circle.SetActive(false);
                }
                //Sluttpunktet til laser strålen
                laserLine.SetPosition(1, hit.point);
                //Når A-knappen på høyre kontroller trykkes inn
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    //Hvis det pekes på menyen, ikke gjør noe
                    if (hit.collider.tag == "VirtualUI")
                        return;
                    //Noen statiske objekter som har collider, kan pekes på direkte
                    //slikt at en teleporterer direkte til dem, men sirkelen blir ikke
                    //synlig. 
                    if (hit.collider.tag != "Plane")
                    {
                        //så en ikke kan gå på toppen av et 3D-modell
                        if (hit.point.y > 0.0f)
                        {
                            transform.localPosition = new Vector3(hit.point.x, 0, hit.point.z) + new Vector3(hit.normal.x, 0, hit.normal.z) * 1.3f; ;
                        }
                        else
                            transform.localPosition = new Vector3(hit.point.x, 0, hit.point.z) + hit.normal * 1.3f; ;

                    }
                    //Hvis det er gulvet som pekes, endre x og z verdiene til kameraet
                    //basert på hit punktet raycaster får. Unity bruker Y-akse for å
                    //definere høyde-akse i 3D-rommet. Siden en bruker ikke skal ha 
                    //muligheten til å flyte i lufta, settes denne verdien til 0.
                    else
                    {
                        transform.localPosition = new Vector3(hit.point.x, 0, hit.point.z);
                    }

                }
            }
            else
            {
                laserLine.SetPosition(1, rightHand.transform.localPosition + (rightHand.transform.forward * rayCastRange));
            }
        }
        //Hvis pekefingeren er nærme pekefingeravtrekkeren så deaktiveres 
        //sirkelen og strålen.
        else
        {
            laserLine.enabled = false;
            circle.SetActive(false);
        }



    }
}