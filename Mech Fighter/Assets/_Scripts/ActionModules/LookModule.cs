using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class LookModule : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vCam;
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

    void OnLook(InputValue value)
    {
        // allow the AI to control its rotation(look target)
        Debug.Log("OnLook Reached!");
        if (playerInputRef.currentControlScheme == "Keyboard & Mouse")
        {
            
        }
        else // it's a controller
        {

        }
    }

}
