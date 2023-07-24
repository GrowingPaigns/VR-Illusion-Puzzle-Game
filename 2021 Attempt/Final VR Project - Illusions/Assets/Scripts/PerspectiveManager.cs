// Forced Perspective Illusion
// Project from Daniel C Menezes
// V1.32 - 17/09/2019
// GitHub: https://github.com/danielcmcg/Forced-Perspective-Illusion-Mechanic-for-Unity

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using System.Linq;
using static OVRInput;

public class PerspectiveManager : MonoBehaviour {

    //cursor material colors (shift depending on what they are hovering over or if an object is selected)
    public Material yShade;
    public Material rShade;
    public Material defaultShade;
    public Material invisiShade;


    private float cameraHeight = 0;
    private Camera mainCamera;
    private Transform targetForTakenObjects;
    private GameObject takenObject;
    
    private RaycastHit rayHit;
    private Ray raycast;
    private float rayMaxRange = 1000f;
    private LayerMask layerMask = ~(1 << 8);
    private bool isRayTouchingSomething = true;

    private float distanceMultiplier;
    private Vector3 scaleMultiplier;
    private float cosine;
    private float positionCalculation;
    private float lastPositionCalculation = 0;
    private Vector3 lastHitPoint = Vector3.zero;
    private Vector3 lastRotation = Vector3.zero;
    private float lastRotationY;
    private Vector3 centerCorrection = Vector3.zero;
    private float takenObjSize = 0;
    private int takenObjSizeIndex = 0;

    private GameObject cursor;

    //pick up noise
    public AudioSource audioSource;
    public AudioClip pop;
    public AudioClip deepPop;
    public float volume = 0.5f;

   

    void Start()
    {

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        targetForTakenObjects = GameObject.Find("targetForTakenObjects").transform;
        
        //positions cursor according to ray 
        cursor = GameObject.Find("Pointer");
        cursor.transform.position = mainCamera.ScreenToWorldPoint(new Vector3((Screen.width / 5) - (Screen.width / 200), (Screen.height / 2) - (Screen.height / 11), 1));
        cursor.transform.parent = mainCamera.transform;
    }

    void Update()
    {
        //sets raycast to the middle of the Oculus HMD
        raycast = mainCamera.ScreenPointToRay(new Vector3((Screen.width / 5) - (Screen.width / 200), (Screen.height / 2) - (Screen.height / 11), 0));
        Debug.DrawRay(raycast.origin, raycast.direction * 200, Color.yellow);
        
        //Changes cursor color depending on what the raycast hits
        if (Physics.Raycast(raycast, out rayHit, rayMaxRange, layerMask))
        {
            if (rayHit.transform.tag == "Getable")
            {
                cursor.GetComponent<MeshRenderer>().material = rShade;
            }
            else
            {
                cursor.GetComponent<MeshRenderer>().material = yShade;
            }

        }

        //checks if raycast is touching a collider, and if it is, it checks the info of the object
        isRayTouchingSomething = Physics.Raycast(raycast, out rayHit, rayMaxRange, layerMask);

        //sets the position of targetForTakenObjects to the point at which the raycast hits a collider 
        if (takenObject != null)
        {
            cursor.GetComponent<MeshRenderer>().material = rShade;
        }
        else
        {
            targetForTakenObjects.position = rayHit.point;
        }

        



        OVRInput.Update();
        if (GetDown(Button.PrimaryHandTrigger) && isRayTouchingSomething)
        {
            
            if (rayHit.transform.tag == "Getable")
            {
                audioSource.PlayOneShot(pop, volume);

                takenObject = rayHit.transform.gameObject; 
                distanceMultiplier = Vector3.Distance(mainCamera.transform.position, takenObject.transform.position);
                scaleMultiplier = takenObject.transform.localScale;
                lastRotation = takenObject.transform.rotation.eulerAngles;
                lastRotationY = lastRotation.y - mainCamera.transform.eulerAngles.y;
                takenObject.transform.transform.parent = targetForTakenObjects;

                //Adds a rigidbody to a picked up obect if necessary (useful for pictures that "hang" on the wall, or similar objects)
                if (takenObject.GetComponent<Rigidbody>() == null)
                {
                    takenObject.AddComponent<Rigidbody>();
                }
                
                //used to reset the (x,y,z) position at which a taken object is eventually dropped
                takenObject.GetComponent<Rigidbody>().isKinematic = true;

                //stops picked up objects from colliding with placed objects while the player looks around 
                foreach (Collider col in takenObject.GetComponents<Collider>())
                {
                    col.isTrigger = true;
                }

                if (takenObject.GetComponent<MeshRenderer>() != null)
                {
                    takenObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    takenObject.GetComponent<MeshRenderer>().receiveShadows = false;
                }

                takenObject.gameObject.layer = 8;

                //keeps any children uniform with picked up object
                foreach (Transform child in takenObject.GetComponentsInChildren<Transform>())
                {
                    takenObject.GetComponent<Rigidbody>().isKinematic = true;
                    takenObject.GetComponent<Collider>().isTrigger = true;
                    child.gameObject.layer = 8;
                }

                //keeps object colliders uniform while picked up object scales (done in realtime (not very efficient))
                takenObjSize = takenObject.GetComponent<Collider>().bounds.size.y;
                takenObjSizeIndex = 1;
                if (takenObject.GetComponent<Collider>().bounds.size.x > takenObjSize)
                {
                    takenObjSize = takenObject.GetComponent<Collider>().bounds.size.x;
                    takenObjSizeIndex = 0;
                }
                if (takenObject.GetComponent<Collider>().bounds.size.z > takenObjSize)
                {
                    takenObjSize = takenObject.GetComponent<Collider>().bounds.size.z;
                    takenObjSizeIndex = 2;
                }
            }
        }

        //the body of the position/size update control
        OVRInput.Update();
        if (Get(Button.PrimaryHandTrigger) && isRayTouchingSomething)
        {
            if (takenObject != null)
            {

                // recenter the object to the center of the mesh regardless of real pivot point when it is picked up
                if (takenObject.GetComponent<MeshRenderer>() != null)
                {
                    centerCorrection = takenObject.transform.position - takenObject.GetComponent<MeshRenderer>().bounds.center;
                }
                
                //uses lerp to determine the distance between the picked up object (which is now kinematic), and the first collision hit with an object that is not kinematic
                takenObject.transform.position = Vector3.Lerp(takenObject.transform.position, targetForTakenObjects.position + centerCorrection, Time.deltaTime * 5);
                //Similarly uses lerp to somewhat rotate the object according to what angle the player views it from (keeps object upright on transformation)
                takenObject.transform.rotation = Quaternion.Lerp(takenObject.transform.rotation, Quaternion.Euler(new Vector3(0, lastRotationY + mainCamera.transform.eulerAngles.y, 0)), Time.deltaTime * 5);


                //uses normal to determine how to orient the taken object to the environment behind it
                cosine = Vector3.Dot(raycast.direction, rayHit.normal);
                cameraHeight = Mathf.Abs(rayHit.distance * cosine);

                //keeps object uniform with it's colliders
                takenObjSize = takenObject.GetComponent<Collider>().bounds.size[takenObjSizeIndex];

                //reduces the size of the object for each 'x' distance that the raycast hit travels (divided by camera height to keep the object centered while shrinking in size)
                positionCalculation = (rayHit.distance * takenObjSize / 2) / cameraHeight;

                if (positionCalculation < rayMaxRange)
                {
                    lastPositionCalculation = positionCalculation;
                }

                // if the wall is more distant then the raycast max range, increase the size only until the max range
                if (isRayTouchingSomething)
                {
                    lastHitPoint = rayHit.point;
                }
                else
                {
                    lastHitPoint = mainCamera.transform.position + raycast.direction * rayMaxRange;
                }

                //recomputes position and scale 
                targetForTakenObjects.position = Vector3.Lerp(targetForTakenObjects.position, lastHitPoint - (raycast.direction * lastPositionCalculation), Time.deltaTime * 10);

                takenObject.transform.localScale = scaleMultiplier * (Vector3.Distance(mainCamera.transform.position, takenObject.transform.position) / distanceMultiplier);
            }
        }

        //drops the object on press (for some reason GetUp was not working)
        OVRInput.Update();
        if (Get(Button.PrimaryIndexTrigger) && isRayTouchingSomething)
        {
            if (takenObject != null)
            {
                audioSource.PlayOneShot(deepPop, volume);

                takenObject.GetComponent<Rigidbody>().isKinematic = false;

                foreach (Collider col in takenObject.GetComponents<Collider>())
                {
                    col.isTrigger = false;
                }

                if (takenObject.GetComponent<MeshRenderer>() != null)
                {
                    takenObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    takenObject.GetComponent<MeshRenderer>().receiveShadows = true;
                }
                takenObject.transform.parent = null;
                takenObject.gameObject.layer = 0;
                foreach (Transform child in takenObject.GetComponentsInChildren<Transform>())
                {
                    takenObject.GetComponent<Rigidbody>().isKinematic = false;
                    takenObject.GetComponent<Collider>().isTrigger = false;
                    child.gameObject.layer = 0;
                }

                takenObject = null;
            }
        }

        

    }
}


