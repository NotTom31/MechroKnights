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


public class GameManager : MonoBehaviour
{
    [Header("Scene Associations")]

    public static GameManager instance;

    public GameState gameState { get; private set; } = GameState.LOADING;
    public delegate void StateChangeHandler(GameState state);
    public static event StateChangeHandler OnStateChange;
    private int charSelected;

    public void setCharacter(int character)
    {
        charSelected = character;
    }

    public void SetCharModel()
    {
        ServiceLocator.GetChangeCharacter().SetChar();
    }

    public int getCharacter()
    {
        return charSelected;
    }

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

        SceneManager.sceneLoaded += SceneLoaded;
    }
    private void Update()
    {
        
    }

    void SceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        SceneManager.sceneUnloaded += SceneUnloaded;
        if (scene.buildIndex == 0)
            SetState(GameState.MAIN_MENU);
        else if (scene.buildIndex == 1)
        {
            MechState.OnHPChange += HPChangeHandler;
            SetState(GameState.PLAYING_ACTIVE);
            GameManager.instance.SetCharModel();
        }
    }
    void SceneUnloaded(Scene scene)
    {
        SceneManager.sceneUnloaded -= SceneUnloaded;
        SceneManager.sceneLoaded += SceneLoaded;
        if (scene.buildIndex == 1)
        {
            MechState.OnHPChange -= HPChangeHandler;
        }
    }
    void HPChangeHandler(float change, int mechIndex)
    {
        if (change > 0)
            return;
        if (mechIndex != 0) // if it's not the player
        {
            SetState(GameState.VICTORY_RESULTS);
            ServiceLocator.GetMenuManager().OpenYouWin();
        }
        else
        {
            SetState(GameState.DEFEAT_RESULTS);
            ServiceLocator.GetMenuManager().OpenYouLose();
        }
    }
    public void SetState(GameState state)
    {
        gameState = state;
        OnStateChange?.Invoke(state);
    }

    
}
