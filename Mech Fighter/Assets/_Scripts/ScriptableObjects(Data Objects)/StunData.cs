using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StunData", menuName = "ScriptableObjects/StunData")]
public class StunData : ScriptableObject
{
    // How much stun certain types of attacks deal
    [Header("Stun Values")]
    public float bulletStun = 50;
    public float heavyStun = 30;
    public float lightStun = 15;

    // Miscellaneous effects that stun has on certain mech actions and other StunSystem values
    [Header("Other")]
    public float decayPerSecond = 2;
    public float movementScale = 0.5f;
    [Range(0, 5)] public float stunDurationSeconds = 0.25f;
}
