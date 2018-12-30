using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class objectCreator : MonoBehaviour {

	
    void objectRead()
    {
        using (FileStream file = File.OpenRead("C:\\Users\\Thor\\Documents\\cube.obj"))
        using (var reader = new StreamReader(file))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line[0] == 'o')
                {
                    //dette er o
                    print("Objektet heter: " + line);
                }
                if(line[0] == 'v')
                {
                    print(line);
                }
            }
        }
    }
    
    
    
    
    
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
