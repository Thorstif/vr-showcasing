using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickTransform : MonoBehaviour {

    public static GameObject activeGameObject = null;
    public GameObject leftController;
    public GameObject rightController;
    public float moveMultiplier = 1;

    Quaternion prevRot;
    Vector3 prevPos;
    Vector3 moveVector;
    Quaternion rotateChange;
    Vector3 startScale;

    float controllerDistance;
    float growth;

    Vector2 rotationXZ;
    Vector2 rotationY;

    public Slider multiSlider;
    public Text multiText;


    // Use this for initialization
    void Start () {
        prevPos = leftController.transform.position;
        prevRot = leftController.transform.localRotation;
    }

    private void OnEnable()
    {
        activeGameObject = ModelBrowser.currentActiveGameObject;
    }

    // Update is called once per frame
    void Update () {

        if (OVRInput.Get(OVRInput.Button.Four)) // Y
        {
            moveVector = new Vector3(leftController.transform.position.x - prevPos.x, leftController.transform.position.y - prevPos.y, leftController.transform.position.z - prevPos.z);
            
            //activeGameObject.transform.position = 
            //    new Vector3(activeGameObject.transform.position.x + moveVector.x * moveVector.x * moveMultiplier, 
            //                activeGameObject.transform.position.y + moveVector.y * moveVector.y * moveMultiplier, 
            //                activeGameObject.transform.position.z + moveVector.z * moveVector.z * moveMultiplier);

            activeGameObject.transform.position =
                new Vector3(activeGameObject.transform.position.x + moveVector.x * moveMultiplier * moveMultiplier,
                            activeGameObject.transform.position.y + moveVector.y * moveMultiplier * moveMultiplier,
                            activeGameObject.transform.position.z + moveVector.z * moveMultiplier * moveMultiplier);
        }

        if (OVRInput.GetDown(OVRInput.Button.Two)) // B
        {
            controllerDistance = CalcContDistance();
            startScale = activeGameObject.transform.localScale;
        }

        if (OVRInput.Get(OVRInput.Button.Two) && ObjectImporter.inProgress == false) // B
        {
            growth = CalcContDistance() / controllerDistance;

            activeGameObject.transform.localScale = 
                new Vector3(startScale.x * growth,
                            startScale.y * growth,
                            startScale.z * growth);
        }


        if (OVRInput.Get(OVRInput.Button.Three)) // X
        {
            rotateChange = leftController.transform.localRotation * Quaternion.Inverse(prevRot);
            activeGameObject.transform.rotation *= rotateChange;

            /*
            rotateChange = leftController.transform.localEulerAngles - prevRot;

            activeGameObject.transform.eulerAngles = 
                new Vector3(activeGameObject.transform.eulerAngles.x + rotateChange.x,
                            activeGameObject.transform.eulerAngles.y + rotateChange.y,
                            activeGameObject.transform.eulerAngles.z + rotateChange.z);
            */
        }

        prevPos = leftController.transform.position;
        prevRot = leftController.transform.localRotation;

        //Henter verdiene fra joystickene
        rotationXZ = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        rotationY = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        if (rotationXZ.x != 0)
        {
             activeGameObject.transform.Rotate(Vector3.right * Time.deltaTime * rotationXZ.x * 100, Space.World);
        }

        if (rotationXZ.y != 0)
        {
            activeGameObject.transform.Rotate(Vector3.forward * Time.deltaTime * rotationXZ.y * 100, Space.World);
        }

        if (rotationY.x != 0)
        {
            activeGameObject.transform.Rotate(Vector3.up * Time.deltaTime * rotationY.x * 100, Space.World);
        }
    }

    float CalcContDistance()
    {
        return Mathf.Sqrt((leftController.transform.position.x - rightController.transform.position.x) *
                          (leftController.transform.position.x - rightController.transform.position.x) +
                          (leftController.transform.position.y - rightController.transform.position.y) *
                          (leftController.transform.position.y - rightController.transform.position.y) +
                          (leftController.transform.position.z - rightController.transform.position.z) *
                          (leftController.transform.position.z - rightController.transform.position.z));
    }

    public void setMultiplierValue()
    {
        moveMultiplier = multiSlider.value;
        multiText.text = moveMultiplier.ToString("#.##");
    }

    //resetter rotasjon
    public void resetRotation()
    {
        activeGameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    //Setter posisjonen til 3D-modellen tilbake til origo
    public void ResetPosition()
    {
        activeGameObject.transform.position = new Vector3(0, 0, 0);
    }

    //resetter skala til original størrelse
    public void ResetScale()
    {
        activeGameObject.transform.localScale = new Vector3(
                    (Mathf.Abs(activeGameObject.transform.localScale.x) / activeGameObject.transform.localScale.x),
                    (Mathf.Abs(activeGameObject.transform.localScale.y) / activeGameObject.transform.localScale.y),
                    (Mathf.Abs(activeGameObject.transform.localScale.z) / activeGameObject.transform.localScale.z));
    }
}
