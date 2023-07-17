using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechState : MonoBehaviour
{
    [SerializeField] public float HP { get; private set; }
    [SerializeField] public float Energy { get; private set; }
    [SerializeField] public int MechIndex { get; private set; }
    [SerializeField][Range(0, 1)] private float energyRegenPercent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        BlockModule.OnEnergyChange += ChangeEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeEnergy(energyRegenPercent * Time.deltaTime, MechIndex);
    }

    private void ChangeEnergy(float change, int index)
    {
        if (index != MechIndex)
            return;
        Energy += change;
    }
}
