using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LookModule : MonoBehaviour
{
    [SerializeField] private Transform playerTransformRef;
    [SerializeField] private PlayerInput playerInputRef;
    [SerializeField] private Camera mainCamera;
    private bool isUsingMouse = false;
    private float calculatedRotationalVelocity;

    private void LateUpdate()
    {
        Quaternion rotation3D = mainCamera.transform.rotation;
        rotation3D.z = 0f;
        rotation3D.x = 0f;
        rotation3D.Normalize();
        playerTransformRef.rotation = rotation3D;
    }

    private void DeviceChangeHandler(InputDevice device, InputDeviceChange ChangeStatus)
    {
        
    }
    void OnLook(InputValue value)
    {
        Debug.Log("OnLook Reached!");
        if (playerInputRef.currentControlScheme == "Keyboard & Mouse")
        {
            
        }
        else // it's a controller
        {

        }
    }

}
