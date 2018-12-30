using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMainScene : MonoBehaviour {
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Backspace))
        {
            ChaseLocation.timed = true;
            ChaseLocation.timeLeft = 5;
            SceneManager.LoadSceneAsync("Main", LoadSceneMode.Single);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadSceneAsync("Start", LoadSceneMode.Single);
        }
    }
}
