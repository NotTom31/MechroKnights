using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBackend : MonoBehaviour
{
    [SerializeField] private Image healthImageRef;
    [SerializeField] private Image energyImageRef;

    void Awake()
    {
        MechState.OnUIEnergyChange += EnergyHandler;
        MechState.OnUIHPChange += HPHandler;

        // maybe grab max values here instead of getting them every change?
    }

    private void HPHandler(float hp, float maxHp, int mechIndex)
    {
        if (mechIndex != 0)
            return;
        healthImageRef.fillAmount = hp / maxHp;
    }
    private void EnergyHandler(float energy, float maxEnergy, int mechIndex)
    {
        if (mechIndex != 0)
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
