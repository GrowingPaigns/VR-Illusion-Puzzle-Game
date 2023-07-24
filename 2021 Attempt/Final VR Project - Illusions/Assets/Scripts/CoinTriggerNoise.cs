using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTriggerNoise : MonoBehaviour
{
    public AudioSource audioSource;
    private bool soundPlayed = false;
    public AudioClip wallBreak;
    public float volume = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (soundPlayed == false)
        {
            audioSource.PlayOneShot(wallBreak, volume);
            soundPlayed = true;
        }
    }
}
