using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public bool isSoundsOn = true;

    public bool isMusicOn = true;

    // 2 is true; 1 is false; 0 is true;
    private bool GetMusic => PlayerPrefs.GetInt("music") == 2 || PlayerPrefs.GetInt("music") == 0;
    private bool GetSound => PlayerPrefs.GetInt("sound") == 2 || PlayerPrefs.GetInt("sound") == 0;

    public static AudioManager Instance { get; private set; }

    // Start is called before the first frame update
    private void Awake()
    {
        isMusicOn = GetMusic;
        isSoundsOn = GetSound;
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        PlayTheme();
    }

    public void PlayTheme()
    {
        if (isMusicOn)
            Play("Theme");
    }

    public void StopTheme()
    {
        var s = Array.Find(sounds, sound => sound.name == "Theme");
        if (s == null)
        {
            Debug.LogWarning("sound is NULL");
            return;
        }

        s.source.Stop();
    }

    public void Play(string name)
    {
        var s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            // Debug.LogWarning("sound is NULL");
            return;
        s.source.Play();
    }

    public void Click()
    {
        if (isSoundsOn)
            Play("Click");
    }

    public void Correct()
    {
        if (isSoundsOn)
            Play("Correct");
    }

    public void Incorrect()
    {
        if (isSoundsOn)
            Play("Incorrect");
    }

    public void SwitchSound(bool isOn)
    {
        isSoundsOn = isOn;
        PlayerPrefs.SetInt("sound", isOn ? 2 : 1);
    }

    public void SwitchMusic(bool isOn)
    {
        isMusicOn = isOn;
        PlayerPrefs.SetInt("music", isOn ? 2 : 1);
        if (isOn) PlayTheme();
        else StopTheme();
    }
}