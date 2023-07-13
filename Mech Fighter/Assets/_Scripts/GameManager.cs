using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator
{
    public SoundManager GetSoundManager()
    {
        return _soundManager;
    }
    public StunSystem GetStunSystem()
    {
        return _stunSystem;
    }

    private static SoundManager _soundManager;
    private static StunSystem _stunSystem;

    public void ProvideService(SoundManager soundManager)
    {
        _soundManager = soundManager;
    }
    public void ProvideService(StunSystem stunSystem)
    {
        _stunSystem = stunSystem;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static ServiceLocator serviceLocator;
    
    // need game state tracking

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
