using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Håndterer veksling mellom miljøer
//Festes til en alltid aktiv skriptholder

public class EnvironmentManager : MonoBehaviour {

	//Skal ha referanse til miljø prefabs
    public GameObject plainEnv;
    public GameObject minimalEnv;
    public GameObject grassEnv;
    public GameObject houseEnv;
    public GameObject cheapWaterEnv;
    public GameObject midWaterEnv;
    public GameObject expensiveWaterEnv;
    public GameObject emptyEnv;

	//skal ha referanse til miljøet det startes med
    public GameObject startEnv;

	//kube som kan slåes av og på, for å gi inntrykk av skala eller som pidestall
    public GameObject cube;

	//skal ha referanse til directinal light som utgjør bakgrunnslyset i scenen
    public GameObject backgroundLight;

	//skal ha referanse til sliders
    public Slider bgLightSlider; //for intensitet til bakgrunnslys
    public Slider dayNightSlider; //for å veksle mellom natt og dag skyboxes
    public Slider includeCubeSlider; //for å slå av og på kube

	//for 2D-meny sin sliders
    public Slider dayNightSlider2; 
    public Slider includeCubeSlider2;

	//referanser til aktivt miljø og kube
    GameObject activeEnvironment = null;
    GameObject activeCube = null;

	//skal ha referanse til materialer for nattehimmel
    public Material nightSkyBox;
    public Material secretSkyBox; //Easteregg
	
	//for å lagre aktiv dag-skybox
    Material defaultSkybox;

	// Use this for initialization
	void Start () {
        activeEnvironment = startEnv;
        defaultSkybox = RenderSettings.skybox;
    }

	//veksling mellom miljøer
    public void plainInstantiateEnv()
    {
        destroyActiveEnv();
        activeEnvironment = Instantiate(plainEnv);
    }

    public void minimalInstantiateEnv()
    {
        destroyActiveEnv();
        activeEnvironment = Instantiate(minimalEnv);
    }

    public void grassInstantiateEnv()
    {
        destroyActiveEnv();
        activeEnvironment = Instantiate(grassEnv);
    }

    public void houseInstantiateEnv()
    {
        destroyActiveEnv();
        activeEnvironment = Instantiate(houseEnv);
    }

    public void cheapWaterInstantiateEnv()
    {
        destroyActiveEnv();
        activeEnvironment = Instantiate(cheapWaterEnv);
    }

    public void midWaterInstantiateEnv()
    {
        destroyActiveEnv();
        activeEnvironment = Instantiate(midWaterEnv);
    }

    public void expensiveWaterInstantiateEnv()
    {
        destroyActiveEnv();
        activeEnvironment = Instantiate(expensiveWaterEnv);
    }

    public void emptyInstantiateEnv()
    {
        destroyActiveEnv();
        activeEnvironment = Instantiate(emptyEnv);
    }

	//Fjerner det aktive miljøet fra scenen
    public void destroyActiveEnv()
    {
        if(startEnv.activeSelf == true)
        {
            startEnv.SetActive(false);
            activeEnvironment = null;
        }

        else
        {
            if (activeEnvironment != null)
            {
                Destroy(activeEnvironment);
            }
        }      
    }
    
	//slår av og på kube
    public void IncludeCube()
    {
        if(includeCubeSlider.value == 0)
        {
            activeCube = Instantiate(cube);
        }

        else
        {
            if(activeCube != null)
            {
                Destroy(activeCube);
            }
        }
    }

	//bytter mellom himmel for natt og dag
    public void SwapSkybox()
    {
        if (dayNightSlider.value == 1)
        {
            RenderSettings.skybox = nightSkyBox;
        }

        else
        {
            RenderSettings.skybox = defaultSkybox;
        }
    }

	//slår av og på kube fra den andre miljø-menyen
    public void IncludeCube2()
    {
        if (includeCubeSlider2.value == 0)
        {
            activeCube = Instantiate(cube);
        }

        else
        {
            if (activeCube != null)
            {
                Destroy(activeCube);
            }
        }
    }

	//bytter mellom himmel for natt og dag fra den andre miljø-menyen
    public void SwapSkybox2()
    {
        if (dayNightSlider2.value == 1)
        {
            RenderSettings.skybox = nightSkyBox;
        }

        else
        {
            RenderSettings.skybox = defaultSkybox;
        }
    }

	//slår på easteregg skybox
    public void hiddenSkybox()
    {
        RenderSettings.skybox = secretSkyBox;
    }

	//for endring av intensitet til bakgrunnslys i scenen
    public void setBackgroundLight()
    {
        Light lightComponent = backgroundLight.GetComponent<Light>();
        lightComponent.intensity = bgLightSlider.value;
    }
}