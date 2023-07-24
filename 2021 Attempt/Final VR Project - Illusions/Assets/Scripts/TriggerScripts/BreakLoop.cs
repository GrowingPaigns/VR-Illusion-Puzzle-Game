using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakLoop : MonoBehaviour
{
    private GameObject wallExit;
    private GameObject wallObscure;
    private GameObject infWall2;
    private GameObject infHall;
    private GameObject drop;

    // Start is called before the first frame update
    void Start()
    {
        wallExit = GameObject.Find("Wall4DExit");
        wallObscure = GameObject.Find("Wall4DObscure");
        infWall2 = GameObject.Find("WallInfiniteHall2");
        infHall = GameObject.Find("InfiniteHallway");
        drop = GameObject.Find("DropTunnel");
        infWall2.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        wallExit.SetActive(false);
        wallObscure.SetActive(true);
        infWall2.SetActive(true);
        infHall.SetActive(false);
        drop.SetActive(false);
    }
}
