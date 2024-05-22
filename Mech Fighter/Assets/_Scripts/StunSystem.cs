using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StunSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MechState mechStateRef;
    [SerializeField] private StunData stunDataRef;
    [SerializeField] private PlayerInput playerInputRef;
    private AICharacter aiCharacterRef;

    private const float STUN_THRESHOLD = 100f;
    private float currentStunValue = 0f;
    private float currentStunnedSeconds = 0f;
    private bool isBot = false;
    
    public bool IsStunned { get; private set; } = false;

    public delegate void StunChangeHandler(bool status, int mechIndex, float currentStunVal);
    public static event StunChangeHandler OnStunChange;

    private void Awake()
    {
        MechState.OnHitBoxHit += HandleHit;
        if (playerInputRef == null)
        {
            aiCharacterRef = gameObject.GetComponent<AICharacter>();
            isBot = true;
        }

    }

    private void Update()
    {
        StunCheck(currentStunValue, currentStunnedSeconds);
        if (!IsStunned)
            currentStunValue = Mathf.Clamp(currentStunValue - (stunDataRef.decayPerSecond * Time.deltaTime), 0.0f, 100f);
        currentStunnedSeconds -= Time.deltaTime;
    }

    private void HandleHit(int mechIndex, bool isBullet, bool isBlocking)
    {
        if (mechIndex != mechStateRef.GetMechIndex())
            return;
        if (isBlocking)
            return;
        if (IsStunned)
            return;

        if (isBullet)
        {
            currentStunValue = Mathf.Clamp(currentStunValue + stunDataRef.bulletStun, 0.0f, 100f);
        }
        else // need more conditions/tests for whether it's a heavy attack
        {
            currentStunValue = Mathf.Clamp(currentStunValue + stunDataRef.lightStun, 0.0f, 100f);
        }
        Debug.Log($"Mech {mechStateRef.GetMechIndex()} has been hit!\n" +
                  $"StunValue: {currentStunValue}\n" +
                  $"Health: {mechStateRef.HP}");
    }

    private void StunCheck(float currentStun, float currentTime)
    {
        if (currentTime <= 0f)
        {
            IsStunned = false;
            if (!isBot)
                playerInputRef.ActivateInput();
            currentStunValue = 0f;
        }
        if (currentStun < STUN_THRESHOLD)
            return;

        IsStunned = true;
        if (!isBot)
            playerInputRef.DeactivateInput();
        currentStunnedSeconds = stunDataRef.stunDurationSeconds;
        OnStunChange?.Invoke(IsStunned, mechStateRef.GetMechIndex(), currentStunValue);
        Debug.Log($"Mech {mechStateRef.GetMechIndex()} has been stunned!\nIsStunned: {IsStunned}\n");
    }

    public float MoveScale()
    {
        return stunDataRef.movementScale;
    }
}
