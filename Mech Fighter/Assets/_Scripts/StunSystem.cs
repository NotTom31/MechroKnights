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
        else // need more conditions/tests for whether it's a heavy attack
        {
            currentStunValue += stunDataRef.lightStun;
        }
    }

    public float MoveScale()
    {
        return stunDataRef.movementScale;
    }
}
