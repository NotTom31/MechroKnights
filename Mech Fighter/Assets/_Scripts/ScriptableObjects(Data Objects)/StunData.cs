using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StunData", menuName = "ScriptableObjects/StunData")]
public class StunData : ScriptableObject
{
    [Header("Stun Values")]
    public float bulletStun = 50;
    public float heavyStun = 30;
    public float lightStun = 15;

    [Header("Other")]
    public float decayPerSecond = 2;
    public float movementScale = 0.5f;
    [Range(0, 5)] public float stunDurationSeconds = 0.25f;
}
