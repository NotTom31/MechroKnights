using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _musicSource, _effectSource;

    [Header("SFX")]
    [SerializeField] AudioClip test;

    [Header("MenuSFX")]
    [SerializeField] AudioClip menuNext;
    [SerializeField] AudioClip menuBack;
    [SerializeField] AudioClip menuPause;

    [Header("Music")]
    [SerializeField] AudioClip menuMusic;
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
            {"stage1Music", stage1Music },
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
    }

    private void Start()
    {
        PlayMusic("menuMusic"); // Placeholder
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
