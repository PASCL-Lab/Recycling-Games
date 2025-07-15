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
    }

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

    public void ToggleBGMusicMute()
    {
        if (bgMusicSource.mute)
            bgMusicSource.mute = false;
        else
            bgMusicSource.mute = true;
    }

    public void ToggleSFXMute()
    {
        if (sfxSource.mute)
            sfxSource.mute = false;
        else
            sfxSource.mute = true;
    }

    public void PlayBGMusic( bool loop = true)
    {
        if (bgMusicSource != null && ! bgMusicSource.mute)
        {
            //bgMusicSource.clip = clip;
            bgMusicSource.loop = loop;
            bgMusicSource.enabled = true;
            bgMusicSource.Play();
        }
    }
}
