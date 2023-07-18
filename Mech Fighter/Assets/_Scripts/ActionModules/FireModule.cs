using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireModule : MonoBehaviour
{
    [SerializeField] private GameObject spawnpoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject fakeCamera;
    [SerializeField] private Transform projectilePool;
    [SerializeField] private Vector3 projectileOffset;
    private bool isAIControl = false;


    // Start is called before the first frame update
    void Awake()
    {
        spawnpoint.transform.position = projectileOffset;
        if (gameObject.GetComponent<PlayerInput>() == null && gameObject.GetComponentInChildren<PlayerInput>() == null)
            isAIControl = true;
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void OnFire()
    {
        RaycastHit hitInfo;
        Vector3 rotationVector;
        Quaternion rotationResult = new();

        if (!isAIControl)
        {
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo);
        }
        else
        {
            Physics.Raycast(fakeCamera.transform.position, fakeCamera.transform.forward, out hitInfo);
        }

        rotationVector = hitInfo.point - spawnpoint.transform.position;
        rotationResult.SetLookRotation(rotationVector);
        Instantiate(projectilePrefab, spawnpoint.transform.position, rotationResult, projectilePool);
    }
}
