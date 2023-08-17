using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/SoundData")]
public class SoundData : ScriptableObject
{
    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip characterSelect;
    public AudioClip stage1;

    [Header("SFX")][Space]
    public AudioClip getReady;
    public AudioClip getReady2;
    public AudioClip startingBell;
    [Space]
    public AudioClip bigHit;
    public AudioClip hit1;
    public AudioClip hit2;
    public AudioClip impactA;
    public AudioClip impactB;
    public AudioClip impactC;
    public AudioClip impactD;
    public AudioClip stun;
    public AudioClip alarm1;
    public AudioClip enemyExplosion;
    public AudioClip explosion;
    public AudioClip explosion2;
    public AudioClip ReelingA;
    [Space]
    public AudioClip heavyCannon;
    public AudioClip heavyCannon2;
    public AudioClip midCannon;

    [Header("UI SFX")]
    public AudioClip menuNext;
    public AudioClip menuSelection;
    public AudioClip gameStart;
    public AudioClip menuBack;
    public AudioClip menuPause;
}
