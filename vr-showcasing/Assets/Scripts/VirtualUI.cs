using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//roterer spillobjekt slik at det alltid vil orienteres mot et kamera
//kan settes på en meny i world space med referanse til vr-bruker-hode-kamera
//meny må festes til til spillobjektet med kamera
//menyen vil da følge hode-rotasjon og alltid vises mot kamera

public class VirtualUI : MonoBehaviour {

    Camera camera;
    Vector3 projectedLookDirection;

    // Use this for initialization
    void Start () {
		//henter kamera-komponent i spillobjekt-referansen
        camera = transform.GetComponentInParent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        projectedLookDirection = Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up);
        transform.rotation = Quaternion.LookRotation(projectedLookDirection);
    }
}
