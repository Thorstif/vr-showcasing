using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

//For visning av av 3d-modeller i modell-utforsker
//Festes til kanvas med nødvendige uielementer

public class ModelBrowser : MonoBehaviour
{
	//skal ha Referanser til UI elementer for det tilhørende kanvaset
    public GameObject btnModel;
    public GameObject modelPanel;
    public Text textObject;
	
    public static string currentActiveModel = "";
    public static GameObject currentActiveGameObject = null; //referanse til valgt 3D-modell

    FindModels findModel;

    // Use this for initialization
    void Start()
    {
        currentActiveModel = "";

		//trenger FindModels komponent for å ta i bruk FindModels-skriptet
        findModel = this.gameObject.AddComponent<FindModels>();
    }

	//Når spillobjektet skriptet er festet til blir aktivt
    void OnEnable()
    {
        LoadModels();
    }

	//Henter referanser til modeller og lager knapper for dem
    public void LoadModels()
    {
        DestroyButtons(modelPanel); //fjerner knapper som finnes fra før

		//Henter navn til modeller
        GameObject[] models = GameObject.FindGameObjectsWithTag("3d-modell");
        List<string> modelNames = new List<string>();

        foreach (GameObject go in models)
        {
            modelNames.Add(go.transform.name);
        }

        string[] modelNamesArray = modelNames.ToArray();

		//lager knapp for hver modell
        for (int i = 0; i < models.Length; i++)
        {
                string selectedModel = modelNamesArray[i];
                GameObject fileButton = (GameObject)Instantiate(btnModel);
                fileButton.GetComponentInChildren<Text>().text = modelNamesArray[i];
                fileButton.transform.SetParent(modelPanel.transform, false);
                Button.ButtonClickedEvent e = new Button.ButtonClickedEvent();
                e.AddListener(() =>
                {
                    currentActiveModel = selectedModel;
                    textObject.text = selectedModel;
                    currentActiveGameObject = findModel.passModel(currentActiveModel);
                });
                fileButton.GetComponent<Button>().onClick = e;
        }
    }

	//Sletter knapper
    void DestroyButtons(GameObject panel)
    {
        foreach (Transform t in panel.transform)
        {
            Destroy(t.gameObject);
        }
    }

    //Sletter valgt modell
    public void DeleteModel()
    {
        if (currentActiveGameObject != null && ObjectImporter.inProgress == false)
        {
            Destroy(currentActiveGameObject);

            currentActiveModel = "";
            currentActiveGameObject = null;

            LoadModels();
        }

        
    }
}