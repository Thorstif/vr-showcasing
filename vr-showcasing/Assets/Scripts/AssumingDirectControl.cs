using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Control "VRchar" with WASD and arrow-keys, forward direction is the direction the camera faces

public class AssumingDirectControl : MonoBehaviour {

    public GameObject VRchar;
    public GameObject face;
    public bool controlAssumed = false;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            controlAssumed = !controlAssumed;

            if(!controlAssumed)
            {
                VRchar.transform.position = new Vector3(0, 0, -2);
                VRchar.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }

        if(controlAssumed)
        {
            if (Input.GetKey(KeyCode.W))
            {
                VRchar.transform.position += face.transform.forward * Time.deltaTime * 3;
            }

            if (Input.GetKey(KeyCode.A))
            {
                VRchar.transform.position += face.transform.right * Time.deltaTime * -3;
            }

            if (Input.GetKey(KeyCode.S))
            {
                VRchar.transform.position += face.transform.forward * Time.deltaTime * -3;    
            }

            if (Input.GetKey(KeyCode.D))
            {
                VRchar.transform.position += face.transform.right * Time.deltaTime * 3;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                VRchar.transform.position += face.transform.up * Time.deltaTime * 3;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                VRchar.transform.position += face.transform.up * Time.deltaTime * -3;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                VRchar.transform.Rotate(0, -5 * Time.deltaTime * 40, 0);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                VRchar.transform.Rotate(VRchar.transform.right * -1 * Time.deltaTime * 50, Space.World);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                VRchar.transform.Rotate(0,5 * Time.deltaTime * 40, 0);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                VRchar.transform.Rotate(VRchar.transform.right * Time.deltaTime * 50, Space.World);
            }
        }
    }
}