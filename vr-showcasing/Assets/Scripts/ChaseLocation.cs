using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attached gameobject will follow "chaseTarget" and orient so it will always face "player"

public class ChaseLocation : MonoBehaviour {

    public GameObject player;
    public GameObject chaseTarget;
    public float minRange = 1;
    public float maxRange = 2;
    float currentRange;

    private bool chase = false;
    private float chaseSpeed = 1;
    private float p, angle;

    private Vector3 velocity = Vector3.zero;

    public float chaseSpeedMultiplier = 1;
    Vector3 moveTo;

    Camera camera;
    Vector3 projectedLookDirection;

    public static bool timed = true;

    public static float timeLeft = 5;

    // Use this for initialization
    void Start () {
        camera = player.transform.GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {

        //distance between attached gameobject and "chaseTarget"
        currentRange = Mathf.Sqrt(  (this.transform.position.x - chaseTarget.transform.position.x) *
                                    (this.transform.position.x - chaseTarget.transform.position.x) +
                                    (this.transform.position.z - chaseTarget.transform.position.z) *
                                    (this.transform.position.z - chaseTarget.transform.position.z));

        if(currentRange > maxRange)
        {
            chase = true;
        }

        if(currentRange < minRange)
        {
            chase = false;
        }

        if(chase == true)
        {
            p = minRange / currentRange;

            moveTo = new Vector3(((this.transform.position.x - chaseTarget.transform.position.x) * p) + chaseTarget.transform.position.x, 
                this.transform.position.y, ((this.transform.position.z - chaseTarget.transform.position.z) * p) + chaseTarget.transform.position.z);

             this.transform.position = Vector3.SmoothDamp(transform.position, moveTo, ref velocity , 1/chaseSpeedMultiplier);

            //angle = Vector3.Angle(player.transform.forward, (this.transform.position - player.transform.position)); // for å justere rotasjon litt

            projectedLookDirection = Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up);
            transform.rotation = Quaternion.LookRotation(projectedLookDirection);

            //old code, not smooth
            #region old
            /*
            chaseSpeed = currentRange * Time.deltaTime * chaseSpeedMultiplier;

            if(this.transform.position.x > chaseTarget.transform.position.x)
            {
                this.transform.position = new Vector3(this.transform.position.x - chaseSpeed, this.transform.position.y, this.transform.position.z);
            }
            else
            {
                this.transform.position = new Vector3(this.transform.position.x + chaseSpeed, this.transform.position.y, this.transform.position.z);
            }

            if (this.transform.position.z > chaseTarget.transform.position.z)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - chaseSpeed);
            }
            else
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + chaseSpeed);
            }
            */
            #endregion
        }

        else
        {
            if (timed == true)
            {
                timeLeft -= Time.deltaTime;

                if (timeLeft < 0)
                {
                    timed = false;
                }
            }
        }
    }
}
