using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteHall : MonoBehaviour
{
    private GameObject infWall;
    private GameObject infWall2;
    private GameObject infWall3;

    private GameObject entryWall;
    private GameObject entryWall2;
    private GameObject entryWall3;
    private GameObject breakLoop;



    // Start is called before the first frame update
    void Start()
    {
        infWall = GameObject.Find("Wall4DObscure");
        infWall2 = GameObject.Find("WallInfiniteHall2");
        infWall3 = GameObject.Find("WallInfiniteHall3");
        infWall.SetActive(true);
        infWall2.SetActive(true);
        infWall3.SetActive(true);

        breakLoop = GameObject.Find("BreakLoop");
        breakLoop.SetActive(false);
        
        


        entryWall = GameObject.Find("Wall4DE");
        entryWall3 = GameObject.Find("Wall4DE (1)");
        entryWall2 = GameObject.Find("Wall4DEx");

    }

    private void OnTriggerEnter(Collider other)
    {
        entryWall.SetActive(true);
        entryWall2.SetActive(true);
        entryWall3.SetActive(true);


        infWall.SetActive(false);
        infWall2.SetActive(false);
        infWall3.SetActive(false);

        breakLoop.SetActive(false);
    }
}
