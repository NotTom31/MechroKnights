using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechState : MonoBehaviour
{
    [SerializeField] public float HP { get; private set; }
    [SerializeField] private float maxHP = 100;
    [SerializeField] public float Energy { get; private set; }
    [SerializeField] private float maxEnergy = 1;
    [SerializeField] private int MechIndex;
    [SerializeField] [Range(0, 1)] private float energyRegenPercent;

    [SerializeField] private PlayerInput playerInputRef;
    [SerializeField] private BlockModule blockModuleRef;
    private bool isAIControl = false;

    public delegate void EnergyChangeHandler(float energy, int mechIndex);
    public delegate void UIEnergyChangeHandler(float energy, float energyMax, int mechIndex);
    public static event EnergyChangeHandler OnEnergyChange;
    public static event UIEnergyChangeHandler OnUIEnergyChange;

    public delegate void HPChangeHandler(float HP, int mechIndex);
    public static event HPChangeHandler OnHPChange;
    public delegate void UIHPChangeHandler(float HP, float maxHP, int mechIndex);
    public static event UIHPChangeHandler OnUIHPChange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        BlockModule.OnEnergyChange += ChangeEnergy;
        GameManager.OnStateChange += StateChangeHandler;
        // subscribe to fire module energy change event

        if (playerInputRef == null)
            isAIControl = true;

        Energy = maxEnergy;
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeEnergy(energyRegenPercent * Time.deltaTime * maxEnergy, MechIndex);
    }

    private void StateChangeHandler(GameState state)
    {
        switch (state)
        {
        case GameState.VICTORY_RESULTS:
            ToggleInput();
            break;
        case GameState.DEFEAT_RESULTS:
            ToggleInput();
            break;
        case GameState.PLAYING_RESET:
            Energy = maxEnergy;
            HP = maxHP;
            break;
        }
            
    }
    private void ChangeEnergy(float change, int index)
    {
        if (index != MechIndex)
            return;
        Energy += change;
        OnEnergyChange?.Invoke(Energy, MechIndex);
        OnUIEnergyChange?.Invoke(Energy, maxEnergy, MechIndex);
    }

    private void Damage(float damageValue)
    {
        if (!blockModuleRef.IsBlocking)
        {
            HP -= damageValue;
            OnHPChange?.Invoke(HP, MechIndex);
            OnUIHPChange?.Invoke(HP, maxHP, MechIndex);
        }
        Debug.Log(gameObject.name + " has " + HP + "HP left!");
    }
    private void ToggleInput()
    {
        if (isAIControl)
        {
            ; // toggle AI control
        }
        else
        {
            if (playerInputRef.inputIsActive)
                playerInputRef.DeactivateInput();
            else
                playerInputRef.ActivateInput();
        }
    }
    public int GetMechIndex()
    {
        return MechIndex;
    }

    public delegate void HitBoxCollisionHandler(int mechIndex, bool isBullet);
    public static event HitBoxCollisionHandler OnHitBoxHit;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered!");
        if (other.gameObject.layer != 7)
        {
            Debug.Log("Other collider is not hurtbox!");
            return;
        }
        BulletMover otherBullet = other.gameObject.GetComponentInChildren<BulletMover>() != null
            ? other.gameObject.GetComponentInChildren<BulletMover>() : other.gameObject.GetComponentInParent<BulletMover>();
        MeleeModule otherMeleeModule = other.gameObject.GetComponentInChildren<MeleeModule>() != null
            ? other.gameObject.GetComponentInChildren<MeleeModule>() : other.gameObject.GetComponentInParent<MeleeModule>();
        string otherTag = other.gameObject.tag;

        if (otherBullet == null && otherMeleeModule == null)
        {
            Debug.Log("Other collider not able to damage!");
            return;
        }
        if (otherBullet != null && otherMeleeModule == null && !gameObject.CompareTag(otherTag))
        {
            Debug.Log("Other collider is a bullet!");
            OnHitBoxHit?.Invoke(MechIndex, true);
            Damage(otherBullet.GetDamage());
        }
        else if (otherMeleeModule != null && !gameObject.CompareTag(otherTag))
        {
            Debug.Log("Other collider is a melee!");
            OnHitBoxHit?.Invoke(MechIndex, false);
            Damage(otherMeleeModule.GetDamage());
        }
    }
}
