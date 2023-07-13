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
    void OnLook(InputValue value)
    {
        Vector2 readValue = value.Get<Vector2>();
        Debug.Log("OnLook Reached!");
        if (playerInputRef.currentControlScheme == "Keyboard & Mouse")
        {
            
        }
        else // it's a controller
        {

        }
    }

}
