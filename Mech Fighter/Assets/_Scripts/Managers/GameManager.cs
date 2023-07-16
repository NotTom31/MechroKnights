using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { LOADING, 
                        MAIN_MENU, 
                        STAGE_SELECTION, 
                        PLAYING_ACTIVE, 
                        PLAYING_RESET, 
                        VICTORY_RESULTS, 
                        DEFEAT_RESULTS, 
                        PAUSED }
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
public class StateTracker
{
    public GameState gameState { get; private set; } = GameState.LOADING;

    public delegate void StateChangeHandler(GameState state);
    public static event StateChangeHandler OnStateChange;
    public void SetState(GameState state)
    {
        gameState = state;
        OnStateChange?.Invoke(state);
    }
}
public class GameManager : MonoBehaviour
{
    [Header("Scene Associations")]

    public static GameManager instance;
    public static ServiceLocator serviceLocator;
    private static StateTracker stateTracker;
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
        if (stateTracker == null)
        {
            stateTracker = new StateTracker();
        }

        SceneManager.sceneLoaded += SceneLoaded;
    }

    void SceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        //if ()

        /*switch (scene.name) {
            case loading.name: ;
                break;
            case mainMenu.name:;
                break;
            default:;
        }*/
    }

}
