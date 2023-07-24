using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallTP : MonoBehaviour
{
    private GameObject player;
    private GameObject breakLoop;
    private bool isTriggered;


    void Start()
    {
        player = GameObject.Find("PerspectiveManager");
        breakLoop = GameObject.Find("BreakLoop");

    }

    private void Update()
    {
        //transforms player position backwards if isTriggered = true
        if (isTriggered)
        {
            player.transform.position = new Vector3(player.transform.position.x - 5, player.transform.position.y, player.transform.position.z);
            isTriggered = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
        }
       
        breakLoop.SetActive(true);

    }
}
