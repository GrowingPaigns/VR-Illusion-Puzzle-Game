using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    private GameObject ceiling;
    private GameObject wallObscure;

    private GameObject upperHalf;
    private GameObject windowWall;
    private GameObject coin;
    private GameObject coin2;

    // Start is called before the first frame update
    void Start()
    {
        ceiling = GameObject.Find("InvisiCeiling");
        wallObscure = GameObject.Find("Wall4DObscure");

        windowWall = GameObject.Find("WindowWall");
        coin = GameObject.Find("Coin");
        coin2 = GameObject.Find("Coin (1)");

        upperHalf = GameObject.Find("TutorialBuilding");
    }


    private void OnTriggerEnter(Collider other)
    {
        windowWall.SetActive(false);
        coin.SetActive(false);
        coin2.SetActive(false);

        upperHalf.SetActive(false);
        ceiling.SetActive(true);
        wallObscure.SetActive(false);
        wallObscure.GetComponent<Rigidbody>().isKinematic = true;
    }
}
