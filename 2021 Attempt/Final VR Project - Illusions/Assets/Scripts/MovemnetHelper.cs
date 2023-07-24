using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MovemnetHelper : MonoBehaviour
{
    private XRRig XRRig;
    private CharacterController characterController;
    private CharacterControllerDriver driver;

    // Start is called before the first frame update
    void Start()
    {
        XRRig = GetComponent<XRRig>();
        characterController = GetComponent<CharacterController>();
        driver = GetComponent<CharacterControllerDriver>();


    }

    // Update is called once per frame
    void Update()
    {
        UpdateCharacterController();
    }

    protected virtual void UpdateCharacterController()
    {
        if (XRRig == null || characterController == null)
            return;

        var height = Mathf.Clamp(XRRig.cameraInRigSpaceHeight, driver.minHeight, driver.maxHeight);
        

        Vector3 center = XRRig.cameraInRigSpacePos;
        center.y = height / 2f + characterController.skinWidth;


        characterController.height = height;
        characterController.center = center;
    }
}
