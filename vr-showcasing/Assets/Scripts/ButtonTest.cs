using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class ButtonTest : MonoBehaviour {
    private Canvas canvas;
    public string filePath;
	// Use this for initialization
	void Start () {
        //canvas = this.GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
               
	}
    public void File()
    {
        //filePath = EditorUtility.OpenFilePanel("Noe", Application.streamingAssetsPath, "obj");
        print(filePath);
    }
}
