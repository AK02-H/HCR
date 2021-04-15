using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public AudioClip[] sfx;
    public MusicTrack[] music;
    private MusicTrack currentMusic;
    
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Sfx")]
    public AudioClip sfx_UiSelect;
    public AudioClip sfx_UiBeep;
    public AudioClip sfx_UiNavi;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        PlayMusic(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMusic.useCustomLoop)
        {


            if (musicSource.isPlaying)
            {
                if (musicSource.time >= currentMusic.loopEnd)
                {
                    Debug.LogWarning("SHOULD LOOP");
                    musicSource.Play();
                    musicSource.time = currentMusic.loopStart;
                }
            }
        }
    }

    public void PlayMusic(int id)
    {
        musicSource.clip = music[id].audio;
        musicSource.time = 0;
        musicSource.Play();
        currentMusic = music[id];
    }

    //should I just store clips on objects instead?
    public void PlaySfx(int sfxId, float volume = 1)
    {
        sfxSource.PlayOneShot(sfx[sfxId], volume);
    }
    
    public void PlaySfx(AudioClip clip, float volume = 1)
    {
        sfxSource.PlayOneShot(clip, volume);
    }
    
}

[System.Serializable]
public class MusicTrack
{
    public AudioClip audio;

    public bool useCustomLoop = false;
    
    public float loopStart;
    public float loopEnd;
}
