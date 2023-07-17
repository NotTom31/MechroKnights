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

    [Header("Damage Values")]
    [SerializeField] [Range(0, 100)] private float lightDamageValue;
    [SerializeField] [Range(0, 100)] private float heavyDamageValue;

    [Header("Attack Timings")]
    [SerializeField] private float lightDelay;
    [SerializeField] private float lightDuration, heavyDelay, heavyDuration;

    private void Awake()
    {
        
    }
    private void Update()
    {
        
    }
    public void OnMeleeLight()
    {
        AnimatorStateInfo stateInfo = animatorRef.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("CombatIdle") || stateInfo.IsName("Move"))
            animatorRef.SetTrigger("Melee");
        // play the light melee animation
        Debug.Log("light melee!");
    }
    public void OnMeleeHeavy()
    {
        // play the heavy melee animation
        Debug.Log("heavy melee!");
    }
    // the damage will be done by the hitboxes fixed to the mech model
    // give the hit box(es) the damage value
}
