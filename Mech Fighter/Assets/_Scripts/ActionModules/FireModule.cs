using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireModule : MonoBehaviour
{
    [SerializeField] private MechState mechStateRef;
    [SerializeField] private BlockModule blockModuleRef;
    [SerializeField] private GameObject spawnpoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject fakeCamera;
    [SerializeField] private Transform projectilePool;
    [SerializeField] private Vector3 projectileOffset;
    [Tooltip("Energy cost applied immediately upon use.")]
    [SerializeField] [Range(0,1)]private float energyCostPercent;
    private bool isAIControl = false;

    public delegate void EnergyChangedHandler(float percentChangeImmediate, int index);
    public static event EnergyChangedHandler OnProjectileFired;

    // Start is called before the first frame update
    void Awake()
    {
        //spawnpoint.transform.position = projectileOffset;
        if (gameObject.GetComponent<PlayerInput>() == null && gameObject.GetComponentInChildren<PlayerInput>() == null)
            isAIControl = true;
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void OnFire()
    {
        if (mechStateRef.Energy < energyCostPercent)
        {
            // play low energy sound?
            Debug.Log($"current energy too low! Attempted by {this.gameObject.name}.");
            return;
        }
        if (blockModuleRef.IsBlocking)
        {
            // invalid action sound?
            Debug.Log($"Attempt to shoot while blocking by {this.gameObject.name}");
            return;
        }

        RaycastHit hitInfo;
        Vector3 rotationVector;
        Quaternion rotationResult = new();
        if (!isAIControl)
        {
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo);
            int startAudio = Random.Range(0, 2);
            if (startAudio == 0)
                SoundManager.Instance.PlaySound("heavyCannon", 0.5f);
            else
                SoundManager.Instance.PlaySound("heavyCannon2", 0.5f);
        }
        else
        {
            Physics.Raycast(fakeCamera.transform.position, fakeCamera.transform.forward, out hitInfo);
            SoundManager.Instance.PlayEnemySound("midCannon", 0.7f);
        }

        OnProjectileFired?.Invoke(-energyCostPercent, mechStateRef.GetMechIndex());

        rotationVector = hitInfo.point - spawnpoint.transform.position;
        rotationResult.SetLookRotation(rotationVector);
        Instantiate(projectilePrefab, spawnpoint.transform.position, rotationResult, projectilePool);
    }
}
