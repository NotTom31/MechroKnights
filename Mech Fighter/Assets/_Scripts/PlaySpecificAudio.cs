using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySpecificAudio : MonoBehaviour
{

    [SerializeField] private AudioSource thisAudioSource;
    [SerializeField] private string thisSoundName;
    private void Awake()
    {
        SoundManager.OnSoundEvent += HandleSoundEvent;
    }

    private void OnDestroy()
    {
        SoundManager.OnSoundEvent -= HandleSoundEvent;
    }

    private void HandleSoundEvent(string soundName)
    {
        if (soundName == thisSoundName)
        {
            thisAudioSource.Play();
            Debug.Log("Playing sound: " + soundName);
        }
    }

/*    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) //just for testing
        {
            PlayThisAudio();
        }
    }

    public void PlayThisAudio()
    {
        
    }*/
}
