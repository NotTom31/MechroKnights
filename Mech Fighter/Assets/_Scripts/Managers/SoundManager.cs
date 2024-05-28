using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _musicSource, _effectSource, _enemySource;

    

    [Header("SFX")]
    [SerializeField] AudioClip alarm1;
    [SerializeField] AudioClip bigHit;
    [SerializeField] AudioClip enemyExplosion;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip explosion2;
    [SerializeField] AudioClip getReady;
    [SerializeField] AudioClip getReady2;
    [SerializeField] AudioClip hit1;
    [SerializeField] AudioClip hit2;
    [SerializeField] AudioClip startingBell;
    [SerializeField] AudioClip stun;
    [SerializeField] AudioClip heavyCannon;
    [SerializeField] AudioClip heavyCannon2;
    [SerializeField] AudioClip midCannon;
    [SerializeField] AudioClip impactATemp;
    [SerializeField] AudioClip impactBTemp;
    [SerializeField] AudioClip impactCTemp;
    [SerializeField] AudioClip impactDTemp;
    [SerializeField] AudioClip reelingATemp;
    [SerializeField] AudioClip explosionTemp;
    [SerializeField] AudioClip heavyDamageTemp;
    [SerializeField] AudioClip swing;



    [Header("MenuSFX")]
    [SerializeField] AudioClip menuNext;
    [SerializeField] AudioClip menuSelection;
    [SerializeField] AudioClip gameStart;
    [SerializeField] AudioClip menuBack;
    [SerializeField] AudioClip menuPause;

    [Header("Music")]
    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip characterSelect;
    [SerializeField] AudioClip stage1Music;

    private Dictionary<string, AudioClip> SoundList;

    public delegate void SoundEvent(string soundName);
    public static event SoundEvent OnSoundEvent;

    private void Awake()
    {
        SoundList = new Dictionary<string, AudioClip>()
        {
            {"menuNext", menuNext },
            {"menuBack", menuBack },
            {"menuPause", menuPause },
            {"menuMusic", menuMusic },
            {"characterSelect", characterSelect },
            {"stage1Music", stage1Music },
            {"alarm1",alarm1},
            {"bigHit",bigHit},
            {"enemyExplosion",enemyExplosion},
            {"explosion",explosion},
            {"explosion2",explosion2},
            {"getReady",getReady},
            {"getReady2",getReady2},
            {"hit1",hit1},
            {"hit2",hit2},
            {"startingBell",startingBell},
            {"stun",stun},
            {"heavyCannon",heavyCannon},
            {"heavyCannon2",heavyCannon2},
            {"midCannon",midCannon},
            {"menuSelection", menuSelection },
            {"gameStart", gameStart },
            {"impactATemp", impactATemp },
            {"impactBTemp", impactBTemp },
            {"impactCTemp", impactCTemp },
            {"impactDTemp", impactDTemp },
            {"reelingATemp", reelingATemp },
            {"explosionTemp", explosionTemp },
            {"heavyDamageTemp", heavyDamageTemp },
            {"swing", swing }
        };

        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); //if root object is not "dont destroy on load", remove comment
        }
        else
        {
            Destroy(gameObject);
        }

        MechState.OnHitBoxHit += PlayHitboxSound;
    }

/*    public void SetEnemySource()
    {
        _enemySource = _effectSource;
    }*/

    public void PlayHitboxSound(int index, bool isBullet, bool isBlocking)
    {
        Debug.Log("Play a sound!");
        if (index == 0 && isBullet)
            PlaySound("explosion");//player hit by bullet
        else if (index == 0 && !isBullet)
            PlaySound("hit1");//player hit by sword
        else if (index == 1 && isBullet)
            PlayEnemySound("enemyExplosion");//ai hit by bullet
        else if (index == 1 && !isBullet)
            PlayEnemySound("hit2");//ai hit by sword

    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Start")
        {
            PlayMusic("menuMusic"); // Placeholder
        }
    }

    public void PlaySound(string name, float volumeScale)
    {
        if (_effectSource == null)
        {
            Debug.LogError("Effect source is not assigned.");
            return;
        }
        if (SoundList.ContainsKey(name))
        {
            AudioClip sound = SoundList[name];
            _effectSource.PlayOneShot(sound, volumeScale);
        }
        else
        {
            Debug.LogError("Sound not found: " + name);
        }
    }

    public void PlayEnemySound(string name)
    {
        if (_effectSource == null)
        {
            //Debug.LogError("Effect source is not assigned.");
            return;
        }
        if (SoundList.ContainsKey(name))
        {
            AudioClip sound = SoundList[name];
            _effectSource.PlayOneShot(sound); //hotfix should be enemy
        }
        else
        {
            Debug.LogError("Sound not found: " + name);
        }
    }

    public void PlayEnemySound(string name, float volumeScale)
    {
        if (_effectSource == null)
        {
            Debug.LogError("Effect source is not assigned.");
            return;
        }
        if (SoundList.ContainsKey(name))
        {
            AudioClip sound = SoundList[name];
            _effectSource.PlayOneShot(sound); //hotfit
        }
        else
        {
            Debug.LogError("Sound not found: " + name);
        }
    }

    public void PlaySound(string name)
    {
        if (_effectSource == null)
        {
            //Debug.LogError("Effect source is not assigned.");
            return;
        }
        if (SoundList.ContainsKey(name))
        {
            AudioClip sound = SoundList[name];
            _effectSource.PlayOneShot(sound);
        }
        else
        {
            Debug.LogError("Sound not found: " + name);
        }
    }

    /*    void Update() //testing
        {
            if (Input.GetKeyDown(KeyCode.E)) //just for testing
            {
                PlaySpecificSound("pew");
            }
        }*/

    public void PlaySpecificSound(string soundName)
    {
        OnSoundEvent?.Invoke(soundName);
    }

    public void PlayMusic(string name)
    {
        if (_musicSource == null)
        {
            Debug.LogError("Music source is not assigned.");
            return;
        }
        if (SoundList.ContainsKey(name))
        {
            AudioClip music = SoundList[name];
            _musicSource.gameObject.GetComponent<AudioSource>().clip = music;
            _musicSource.Play();
        }
        else
        {
            Debug.LogError("Music not found: " + name);
        }
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

/*    public void PlayDialogueSFX()
    {
        AudioClip clip = dialogueClips[Random.Range(0, dialogueClips.Count)];

        _effectSource.PlayOneShot(clip, 0.3f);
    }*/
}
