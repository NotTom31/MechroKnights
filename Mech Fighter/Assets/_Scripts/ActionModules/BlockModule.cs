using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlockModule : MonoBehaviour
{
    [SerializeField] private MechState mechStateRef;
    [SerializeField] private Collider blockVolume;
    [SerializeField] private Animator animatorRef;
    [SerializeField] [Range(0, 1)] private float energyCostPercent;
    
    public delegate void EnergyChangeHandler(float energyChangePercent, int mechIndex);
    public static event EnergyChangeHandler OnEnergyChange;

    // Start is called before the first frame update
    void Awake()
    {
        blockVolume.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(blockVolume.enabled == true)
        {
            OnEnergyChange?.Invoke(-energyCostPercent * Time.deltaTime, mechStateRef.MechIndex);
        }
    }

    void OnBlock(/*InputValue value*/)
    {
        Debug.Log("Block held!");
        blockVolume.enabled = true;
        animatorRef.SetBool("Is Block", true);
        /*if (value.isPressed)
        {
            blockVolume.enabled = true;
            animatorRef.SetBool("Is Block", true);
        }
        else
        {
            blockVolume.enabled = false;
            animatorRef.SetBool("Is Block", false);
        }*/
    }

}
