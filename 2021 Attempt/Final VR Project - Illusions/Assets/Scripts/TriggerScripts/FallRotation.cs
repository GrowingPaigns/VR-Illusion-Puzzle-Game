using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRotation : MonoBehaviour
{
    private GameObject wall1;
    private GameObject wall2;

    // Start is called before the first frame update
    void Start()
    {
        wall1 = GameObject.Find("Wall4DE");
        wall2 = GameObject.Find("Wall4DEx");
        wall1.SetActive(true);
        wall2.SetActive(true);
        
    }


    private void OnTriggerEnter(Collider other)
    {

        wall1.SetActive(false);
        
        wall2.SetActive(false);
    }
}
