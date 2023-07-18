using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseModule : MonoBehaviour
{
    public MenuManager menuManager;

    public void OnPause(InputValue context)
    {
        if (menuManager.IsPaused())
        {
            menuManager.Unpause();
            // pause animation?
        }
        else
        {
            menuManager.Pause();
            // unpause animation?
        }
    }
}
