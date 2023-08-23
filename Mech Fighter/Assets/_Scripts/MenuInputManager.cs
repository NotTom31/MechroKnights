using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputManager : MonoBehaviour
{
    public static MenuInputManager instance;

    public Vector2 NavigationInput { get; set; }
    //public bool SubmitInput { get; set; }

    private InputAction _navigationAction;
    //private InputAction _submitAction;

    public static PlayerInput PlayerInput { get; set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        PlayerInput = GetComponent<PlayerInput>();
        _navigationAction = PlayerInput.actions["Navigate"];
        //_submitAction = PlayerInput.actions["Submit"];
    }

    private void Update()
    {
        NavigationInput = _navigationAction.ReadValue<Vector2>();
        //SubmitInput = _submitAction.IsInProgress();
    }
}
