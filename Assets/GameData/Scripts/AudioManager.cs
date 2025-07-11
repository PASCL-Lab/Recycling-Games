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
        // Singleton pattern — one AudioManager instance across scenes
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

    /// <summary>
    /// Play a sound effect from the audio clip list by index
    /// </summary>
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

    /// <summary>
    /// Mute or unmute the background music
    /// </summary>
    public void ToggleBGMusicMute(bool isMuted)
    {
        if (bgMusicSource != null)
            bgMusicSource.mute = isMuted;
    }

    /// <summary>
    /// Mute or unmute all other sounds (sound effects)
    /// </summary>
    public void ToggleSFXMute(bool isMuted)
    {
        if (sfxSource != null)
            sfxSource.mute = isMuted;
    }

    /// <summary>
    /// Optional: Play background music if assigned and not already playing
    /// </summary>
    public void PlayBGMusic( bool loop = true)
    {
        if (bgMusicSource != null)
        {
            //bgMusicSource.clip = clip;
            bgMusicSource.loop = loop;
            bgMusicSource.enabled = true;
            bgMusicSource.Play();
        }
    }
}
