using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class LookModule : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private Transform playerTransformRef;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject fakeCamera;
    private bool isAIControl = false;

    private void Awake()
    {
        if (gameObject.GetComponent<PlayerInput>() == null && gameObject.GetComponentInChildren<PlayerInput>() == null)
            isAIControl = true;
    }

    private void LateUpdate()
    {
        Quaternion rotation3D;
        if (!isAIControl)
            rotation3D = mainCamera.transform.rotation;
        else
            rotation3D = fakeCamera.transform.rotation;
        rotation3D.z = 0f;
        rotation3D.x = 0f;
        rotation3D.Normalize();
        playerTransformRef.rotation = rotation3D;
    }

    public void OnLook(GameObject target)
    {
        fakeCamera.transform.LookAt(target.transform);
    }
    void OnLook(InputValue value)
    {
        Debug.Log("OnLook Reached!");
    }
}
