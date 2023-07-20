using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HUDBackend : MonoBehaviour
{
    [SerializeField] private Image healthImageRef;
    [SerializeField] private Image energyImageRef;
    [SerializeField] private MechState mechStateRef;
    private bool isAIControl = false;

    void Awake()
    {
        MechState.OnUIEnergyChange += EnergyHandler;
        MechState.OnUIHPChange += HPHandler;
        if (energyImageRef == null)
            isAIControl = true;
        // maybe grab max values here instead of getting them every change?
    }

    private void HPHandler(float hp, float maxHp, int mechIndex)
    {
        if (mechIndex != mechStateRef.GetMechIndex())
            return;
        healthImageRef.fillAmount = hp / maxHp;
    }
    private void EnergyHandler(float energy, float maxEnergy, int mechIndex)
    {
        if (mechIndex != 0 || isAIControl)
            return;
        energyImageRef.fillAmount = energy / maxEnergy;
    }

    void Update()
    {
        
    }
    private void OnDestroy()
    {
        MechState.OnUIEnergyChange -= EnergyHandler;
        MechState.OnUIHPChange -= HPHandler;
    }
}
