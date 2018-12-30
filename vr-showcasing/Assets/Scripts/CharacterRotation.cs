using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotation : MonoBehaviour
{

    public float rotationSpeed = 5;
    private Vector3 MoveThrottle = Vector3.zero;
    Vector2 secondaryAxis;

    // Use this for initialization
    void Start()
    {
        secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion ort = transform.rotation;
        Vector3 ortEuler = ort.eulerAngles;
        ortEuler.z = ortEuler.x = 0f;
        ort = Quaternion.Euler(ortEuler);
        secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        transform.rotation = Quaternion.Euler(0, rotationSpeed * secondaryAxis.x, 0) * transform.rotation;

        Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        if (primaryAxis.y > 0.0f)
            MoveThrottle += ort * (primaryAxis.y * transform.lossyScale.z * 1.0f * Vector3.forward);

        if (primaryAxis.y < 0.0f)
            MoveThrottle += ort * (Mathf.Abs(primaryAxis.y) * transform.lossyScale.z  * Vector3.back);

        if (primaryAxis.x < 0.0f)
            MoveThrottle += ort * (Mathf.Abs(primaryAxis.x) * transform.lossyScale.x * Vector3.left);

        if (primaryAxis.x > 0.0f)
            MoveThrottle += ort * (primaryAxis.x * transform.lossyScale.x * Vector3.right);

    }
}