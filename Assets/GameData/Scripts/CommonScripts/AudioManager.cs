using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgMusicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public List<AudioClip> audioClips;

    private AudioSource loopingSFXSource;

    //Initialize the audio sources
    private void Awake()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        bgMusicSource = sources[0];
        sfxSource = sources[1];
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        PlayBGMusic(true);
        loopingSFXSource = gameObject.AddComponent<AudioSource>();
        loopingSFXSource.loop = true;
    }

    //Play sound by index of the list of audio clips
    public void PlaySound(int index)
    {
        if (index >= 0 && index < audioClips.Count)
        {
            sfxSource.PlayOneShot(audioClips[index]);
        }
        else
        {
            Debug.LogWarning("Audio clip index out of range!");
        }
    }

    //Mute the BGMusic Source
    public void ToggleBGMusicMute()
    {
        if (bgMusicSource.mute)
        {
            bgMusicSource.mute = false;
        }
        else {
            bgMusicSource.mute = true;
        }
            
    }

    //Mute the Sound Effects Music Source
    public void ToggleSFXMute()
    {
        if (sfxSource.mute)
        {
            sfxSource.mute = false;
            loopingSFXSource.mute = false;
        }
        else
        {
            sfxSource.mute = true;
            loopingSFXSource.mute = true;
        }
        
    }

    // Play BG Music
    public void PlayBGMusic(bool loop = true)
    {
        if (bgMusicSource != null && !bgMusicSource.mute)
        {
            //bgMusicSource.clip = clip;
            bgMusicSource.loop = loop;
            bgMusicSource.enabled = true;
            bgMusicSource.Play();
        }
    }

    // Play Loop audio Source Music
    public void PlaySoundLoop(int index)
    {
        if (index >= 0 && index < audioClips.Count)
        {
            loopingSFXSource.clip = audioClips[index];
            loopingSFXSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio clip index out of range!");
        }
    }

    // Stop Loop audio Source Music
    public void StopSoundLoop()
    {
        if (loopingSFXSource.isPlaying)
        {
            loopingSFXSource.Stop();
        }
    }
}
