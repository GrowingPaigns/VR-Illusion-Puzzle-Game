using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackHallEntry : MonoBehaviour
{
    private GameObject wall;

    private GameObject lowerHalf;

    // Start is called before the first frame update
    void Start()
    {
        wall = GameObject.Find("InvisiWall");

        //lowerHalf = GameObject.Find("SecondBuilding (Below)");
       // lowerHalf.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            wall.SetActive(true);
            wall.GetComponent<Rigidbody>().isKinematic = true;
        }
        //lowerHalf.SetActive(true);
    }
}
