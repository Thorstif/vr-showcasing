using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR = UnityEngine.VR;

//Moves user with left joystick, based on user face direction
//Rotates with right joystick
//Must be attached to a script-keeper
//Commented out lines are for movement independent of face direction

public class CharacterRotate2 : MonoBehaviour {

    public float rotationSpeed = 100f;
    public float speed = 1.0f; //movement speed
    
    public GameObject vrCharacter; //must have reference to the enitre gameobject that is to be controlled
    public GameObject center; //must have reference to use's head movement (vr user camera))
	
	Vector2 secondaryAxis; //rotation with right controller
    Vector2 movement; //movement with left controller
	Vector3 moveDir; //movement direction
    float angle; //face direction

	
	// Update is called once per frame
	void Update () {

		//Checks joystick input
        secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        movement = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
		
		//Rotates based on right joystick
        vrCharacter.transform.rotation = Quaternion.Euler(0, secondaryAxis.x * Time.deltaTime * rotationSpeed, 0)* vrCharacter.transform.rotation;
        
		//Finds face direction
		angle = center.transform.localEulerAngles.y;

		//Forward
        if (movement.y > 0.0f )
        {
            //vrCharacter.transform.Translate(Vector3.forward * speed * Time.deltaTime * -movement.x);
            moveDir = Quaternion.Euler(0, angle, 0)*Vector3.forward;
            vrCharacter.transform.Translate(moveDir * speed * Time.deltaTime * movement.y);         
        }

		//Left
        if (movement.x < 0.0f)
        {
            //vrCharacter.transform.Translate(Vector3.left * speed * Time.deltaTime * -movement.x);
            moveDir = Quaternion.Euler(0, angle, 0) * Vector3.left;
            vrCharacter.transform.Translate(moveDir * speed * Time.deltaTime * -movement.x);
        }

		//Backwards
        if (movement.y < 0.0f)
        {
            //vrCharacter.transform.Translate(Vector3.back * speed * Time.deltaTime * -movement.y);
            moveDir = Quaternion.Euler(0, angle, 0) * Vector3.back;
            vrCharacter.transform.Translate(moveDir * speed * Time.deltaTime * -movement.y);
        }

		//Right
        if (movement.x > 0.0f)
        {
            //vrCharacter.transform.Translate(Vector3.right * speed * Time.deltaTime * movement.x);
            moveDir = Quaternion.Euler(0, angle, 0) * Vector3.right;
            vrCharacter.transform.Translate(moveDir * speed * Time.deltaTime * movement.x);
        }
    }  

    public void ResetPosition()
    {
        vrCharacter.transform.position = new Vector3(0, 0, 0);
    }
}
