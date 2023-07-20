using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeModule : MonoBehaviour
{
    // May use these modules for
    [Header("Module References")]
    [SerializeField] private JumpModule jumpModuleRef;
    [SerializeField] private MoveModule moveModuleRef;
    [SerializeField] private LookModule LookModuleRef;
    [SerializeField] private Animator animatorRef;
    [SerializeField] private Collider weaponColl;

    [Header("Damage Values")]
    [SerializeField] [Range(0, 100)] private float lightDamageValue;
    [SerializeField] [Range(0, 100)] private float heavyDamageValue;

    [Header("Attack Timings")]
    [SerializeField] private float lightDelay;
    [SerializeField] private float lightDuration, heavyDelay, heavyDuration;
    private float currentDamage = 0f;
    private float durationleft = 0f;
    private bool isAiControl = false;


    private void Awake()
    {
        if (gameObject.GetComponent<PlayerInput>() == null && gameObject.GetComponentInChildren<PlayerInput>() == null)
            isAiControl = true;
        weaponColl.enabled = false;
    }
    private void FixedUpdate()
    {
        if (durationleft > 0)
            durationleft -= Time.deltaTime;
        if (durationleft <= 0 && weaponColl.enabled)
            weaponColl.enabled = false;

    }
    public float GetDamage()
    {
        durationleft = 0f;
        return currentDamage;
    }
    public void OnMeleeLight()
    {
        if (isAiControl)
        {
            SoundManager.Instance.PlayEnemySound("swing");
        }
        else
        {
            SoundManager.Instance.PlaySound("swing");
        }
        AnimatorStateInfo stateInfo = animatorRef.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("CombatIdle") || stateInfo.IsName("Move"))
        {
            currentDamage = lightDamageValue;
            durationleft = lightDuration;
            weaponColl.enabled = true;
            animatorRef.SetTrigger("Melee");
        }
        Debug.Log("light melee!");
    }
    public void OnMeleeHeavy()
    {
        if (isAiControl)
        {
            SoundManager.Instance.PlayEnemySound("reelingATemp");
        }
        else
        {
            SoundManager.Instance.PlaySound("swing");
        }
        AnimatorStateInfo stateInfo = animatorRef.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("CombatIdle") || stateInfo.IsName("Move"))
        {
            currentDamage = heavyDamageValue;
            durationleft = heavyDuration;
            weaponColl.enabled = true;
            animatorRef.SetTrigger("Melee 2");
        }
        Debug.Log("heavy melee!");
    }
    // the damage will be done by the hitboxes fixed to the mech model
    // give the hit box(es) the damage value
}
