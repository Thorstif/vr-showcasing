using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvManager : MonoBehaviour {

    static string envName = "Plain";
    static string currentEnv = "Plain";
    //static int envID = 1;

    //string[] environmentNames = {"Plain", "House", "Sea"};

    GameObject environment;

    public static int currentActive = 1;

	// Use this for initialization
	void Start () {
        resetEnv();
        envName = "Plain";
        currentEnv = "Plain";
        environment = transform.Find(currentEnv).gameObject;
        environment.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void envID1()
    {
        envName = "Plain";
        announce();
    }

    public void envID2()
    {
        envName = "House";
        announce();
    }

    public void envID3()
    {
        envName = "Sea";
        announce();
    }

    public void announce()
    {
        print("envID set to: " + envName);
    }


    public void activate()
    {
        environment = transform.Find(currentEnv).gameObject;
        environment.SetActive(false);

        environment = transform.Find(envName).gameObject;
        environment.SetActive(true);

        currentEnv = envName;
    }


    public string getEnvName()
    {
        return envName;
    }

    public void resetEnv()
    {
        for(int i=2; i<4; i++)
        {
            //test(i);
            environment = transform.Find(test(i)).gameObject;
            environment.SetActive(false);
        }
    }

    string test(int a)
    {
        switch (a)
        {
            case 1:
                envName = "Plain";
                break;
            case 2:
                envName = "House";
                break;
            case 3:
                envName = "Sea";
                break;
            default:
                envName = "Plain";
                break;
        }

        return envName;
    }
}
