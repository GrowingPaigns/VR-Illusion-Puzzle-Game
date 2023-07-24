using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeInvisible : MonoBehaviour
{
    
    private GameObject[] invisible;
    private GameObject sign;




    // Start is called before the first frame update
    void Start()
    {
        invisible = GameObject.FindGameObjectsWithTag("1stBuildingInvis");
        sign = GameObject.Find("TriggeredSign");
        sign.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Player")
        {
            foreach(GameObject objects in invisible)
            {
                objects.SetActive(false);
                sign.SetActive(true);
            }
        }
        
        
        
        
    }

   
}
