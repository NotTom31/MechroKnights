using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MechState mechStateRef;
    [SerializeField] private StunData stunDataRef;

    private const float STUN_THRESHOLD = 100f;
    private float currentStunValue = 0f;
    public bool IsStunned { get; private set; } = false;

    private void Awake()
    {
        ServiceLocator.ProvideService(this);
        MechState.OnHitBoxHit += HandleHit;
    }

    private void Update()
    {
        
    }

    private void HandleHit(int mechIndex, bool isBullet)
    {
        if (mechIndex != mechStateRef.GetMechIndex())
            return;

        if (isBullet)
        {
            currentStunValue += stunDataRef.bulletStun;
        }
        //else if () // <--- CONTINUE HERE

    }

    public float MoveScale()
    {
        return stunDataRef.movementScale;
    }
}

[CreateAssetMenu(fileName = "Stun Values", menuName = "ScriptableObjects/StunData")]
public class StunData : ScriptableObject
{
    [Header("Stun Values")]
    public float bulletStun = 50;
    public float heavyStun = 30;
    public float lightStun = 15;

    [Header("Other")]
    public float decayPerSecond = 2;
    public float movementScale = 0.5f;
    [Range(0, 5)] public float stunDurationSeconds = 0.25f;
}
