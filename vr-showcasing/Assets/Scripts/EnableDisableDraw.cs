/**************************************************
 * EnableDisableDraw klassen brukes til å aktiver
 * og deaktivere tegneverktøyet
***************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableDisableDraw : MonoBehaviour {
    public Slider slider;
    public GameObject capsule;
	
    public void EnableDisable()
    {
		//Basert på slider verdien fra tegneverktøy menyen
        //aktiveres/deaktiveres muligheten for tegning
        if(slider.value==1)
		{
			//Aktiverer Draw3D skriptet
            GetComponent<Draw3D>().enabled = true; 
		}
        else
        {
			//Deaktiverer Draw3D skriptet, og skjuler indikasjonen
            //som viser hvor tegningen kommer "ut" fra
            GetComponent<Draw3D>().enabled = false;
            capsule.SetActive(false);
        }
            
    }
}
