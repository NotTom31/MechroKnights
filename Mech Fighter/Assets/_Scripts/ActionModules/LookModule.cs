using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LookModule : MonoBehaviour
{
    [SerializeField] private Transform playerTransformRef;
    [SerializeField] private PlayerInput playerInputRef;
    private bool isUsingMouse = false;
    private float calculatedRotationalVelocity;

    private void Awake()
    {
        
    }
    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
    }

    private void DeviceChangeHandler(InputDevice device, InputDeviceChange ChangeStatus)
    {
        
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 readValue = context.ReadValue<Vector2>();
        Debug.Log("OnLook Reached!");
        if (playerInputRef.currentControlScheme == "Keyboard")
        {
            
        }
        else // it's a controller
        {

        }
    }

}
