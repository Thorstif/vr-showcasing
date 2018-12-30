using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For UI which hovers in worldspace based on face direction and is locked in height
//attaches to intended UI gameobject
//contains remnants of code wor locking in worldspace

public class ClampVirtualUI : MonoBehaviour {
    Vector3 pos;
    //public bool stay = false;
	//public bool worldParent;
    Quaternion rot;
	public float height = 1.5f;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        //if (!stay)
        {
			//gets position
            pos = transform.position;
            //pos.y = Mathf.Clamp(transform.position.y, 1, 1);
			
			//locks height to y
            pos.y = height;
            transform.position = pos;
        }
		
        /*
        else
        {
            if(OVRInput.GetDown(OVRInput.Button.Start))
            {
                setInPlace();
            }

            rot = transform.rotation;
            transform.rotation = rot;
        }
        */
    }

	/*
    void setInPlace()
    {
        pos = transform.position;
		transform.SetParent(worldParent.transform, true);
        transform.position = pos;  
    }
	*/
}