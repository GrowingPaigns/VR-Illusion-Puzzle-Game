using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BRTrigger : MonoBehaviour
{
    private GameObject wall;

    // Start is called before the first frame update
    void Start()
    {
        wall = GameObject.Find("Wall4DExit");
    }


    private void OnTriggerEnter(Collider other)
    {
        wall.SetActive(true);
    }
}
