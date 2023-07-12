using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySpecificAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thisAudioSource;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) //just for testing
        {
            PlayThisAudio();
        }
    }

    public void PlayThisAudio()
    {
        thisAudioSource.Play();
    }
}
