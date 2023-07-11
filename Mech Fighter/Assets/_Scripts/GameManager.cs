using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class ServiceLocator
{
    public AudioMixer GetAudioMixer()
    {
        return _audioMixer;
    }
    public StunSystem GetStunSystem()
    {
        return _stunSystem;
    }

    private static AudioMixer _audioMixer;
    private static StunSystem _stunSystem;

    public void ProvideServices(AudioMixer audioMixer, StunSystem stunSystem)
    {
        _audioMixer = audioMixer;
        _stunSystem = stunSystem;

    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static ServiceLocator serviceLocator;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
            return; // Not sure if destroying will stop this method, so this is here just in case it doesn't.
        }
        if (serviceLocator == null)
        {
            serviceLocator = new ServiceLocator();
        }
    }


}
