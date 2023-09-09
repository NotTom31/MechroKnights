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
    private float currentStunnedSeconds = 0f;
    
    public bool IsStunned { get; private set; } = false;

    public delegate void StunChangeHandler(bool status, int mechIndex, float currentStunVal);
    public static event StunChangeHandler OnStunChange;

    private void Awake()
    {
        ServiceLocator.ProvideService(this);
        MechState.OnHitBoxHit += HandleHit;
    }

    private void Update()
    {
        currentStunValue = Mathf.Clamp(currentStunValue - stunDataRef.decayPerSecond, 0.0f, 100f);
        currentStunnedSeconds -= Time.deltaTime;
    }

    private void HandleHit(int mechIndex, bool isBullet)
    {
        if (mechIndex != mechStateRef.GetMechIndex())
            return;

        if (isBullet)
        {
            currentStunValue = Mathf.Clamp(currentStunValue + stunDataRef.bulletStun, 0.0f, 100f);
        }
        else // need more conditions/tests for whether it's a heavy attack
        {
            currentStunValue = Mathf.Clamp(currentStunValue + stunDataRef.lightStun, 0.0f, 100f);
        }

    }

    private void StunCheck(float currentStun, float currentTime)
    {
        if (currentTime <= 0)
            IsStunned = false;
        if (currentStun < STUN_THRESHOLD)
            return;

        IsStunned = true;
        currentStunnedSeconds = stunDataRef.stunDurationSeconds;
        OnStunChange?.Invoke(IsStunned, mechStateRef.GetMechIndex(), currentStunValue);
    }
    public float MoveScale()
    {
        return stunDataRef.movementScale;
    }
}
