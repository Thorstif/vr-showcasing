using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages changing between canvases/menus for computerscreen (2D).
//Must be attached to a "script-keeper" which can be referenced with buttons.

public class ButtonScripts : MonoBehaviour {
   
    GameObject canvas,canvas2, fileBrowser2D;

    // Use this for initialization
    void Start () {
        canvas = transform.Find("Canvas1").gameObject;
        canvas2 = transform.Find("Canvas2").gameObject;
        fileBrowser2D = transform.Find("FileBrowser2D").gameObject;

        canvas.SetActive(true);
    }

    //Changes from Main menu to file browser
    public void File()
    {
        canvas.SetActive(false);
        fileBrowser2D.SetActive(true);
    }

	//Testmethod
    public void LoadEnv()
    {
        Debug.Log("Load Env");
    }
    
	//Quits application
    public void CloseProgram()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

	//From Main menu to environment menu
    public void ChangeEnvironmentMenu()
    {
        canvas.SetActive(false);
        canvas2.SetActive(true);
        Debug.Log("Change Env");
    }

	//returns from file browser or environment menu to main menu
    public void Return()
    {
        canvas2.SetActive(false);
        fileBrowser2D.SetActive(false);
        canvas.SetActive(true);
    }

	//Testmethod
    public void Apply()
    {
        Return();
        Debug.Log("Apply");
    }
}
