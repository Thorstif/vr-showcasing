using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Deletes active gameobject defined in the FindModels class
public class DeleteActiveModel : MonoBehaviour
{
    FindModels findModel;

    void Start()
    {
        //must have refereanse to class through component added here
        findModel = this.gameObject.AddComponent<FindModels>();
    }

    public void deleteActiveModel()
    {
        string activeModelName = ModelBrowser.currentActiveModel;

        if(activeModelName != "")
        {           
            GameObject activeGameObject = findModel.passModel(activeModelName);
            Destroy(activeGameObject.gameObject);
        }    
    }
}
