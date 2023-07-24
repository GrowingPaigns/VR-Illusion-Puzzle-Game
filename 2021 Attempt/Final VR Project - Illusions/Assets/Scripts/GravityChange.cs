using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChange : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject playerTransform;
    public float xAngle, yAngle, zAngle;

    void Start()
    {
        playerTransform = GameObject.Find("PerspectiveManager");
        
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Physics.gravity = new Vector3(0, 10, 0);
            playerTransform.transform.Rotate(180, 0, 0, Space.Self);
            playerTransform.transform.Rotate(180, 0, 0, Space.World);
        }
    }
}
