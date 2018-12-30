using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Finner modeller basert på modellene sine navn

public class FindModels : MonoBehaviour
{

    public static string modelName;

	//testmetode for å sjekke tilgjengelige modeller i konsollen
    void findModels()
    {
        GameObject[] models = GameObject.FindGameObjectsWithTag("3d-modell");
        //List<string> modelNames = new List<string>();

        foreach (GameObject go in models)
        {
            print(go.transform.name);
            //modelNames.Add(go.transform.name);
        }
    }

	//finner modell ved navn og returnerer en int som er index til modell i tabell
    int findModelOfName(string modelName)
    {
		//Lager en tabell av alle modeller med 3d-modell taggen
        GameObject[] models = GameObject.FindGameObjectsWithTag("3d-modell");

        for (int i = 0; i < models.Length; i++)
        {
			//letter etter modell med navnet
            if (models[i].transform.name == modelName)
            {
                return i;
            }
        }

        return 0;
    }

	//returnerer referanse til spillobjekt fra tabellen
    public GameObject passModel(string modelName)
    {
        int modelIndex;

        modelIndex = findModelOfName(modelName);

        GameObject[] models = GameObject.FindGameObjectsWithTag("3d-modell");

        return models[modelIndex];
    }
}
