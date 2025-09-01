
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{
    public Image targetImage; // The Image component whose sprite you want to change
    public Sprite newSprite; // Sprite to switch to (e.g., when muted)
    public Sprite DefaultSprite; // Original/default sprite
    public Audiotype audioType; // Type of audio this button controls
    AudioSource audioSource; // Reference to the relevant AudioSource

    // Enum to specify whether the button controls SFX or BG Music
    public enum Audiotype
    {
        SFX,
        BGMusic
    }

    private void Awake()
    {
        // Assign the correct AudioSource depending on type
        if (audioType == Audiotype.SFX)
        {
            audioSource = AudioManager.Instance.sfxSource;
        }
        else
        {
            audioSource = AudioManager.Instance.bgMusicSource;
        }
        // Set the correct sprite according to the current mute state
        CorrectSprite();
    }

    // Sets the sprite according to whether the AudioSource is muted
    void CorrectSprite()
    {
        if (audioSource.mute)
        {
            targetImage.sprite = newSprite;
        }
        else
        {
            targetImage.sprite = DefaultSprite;
        }
    }

    // Switches between default and new sprite when the button is clicked
    public void ChangeSprite()
    {
        if (targetImage.sprite == DefaultSprite)
        {
            targetImage.sprite = newSprite;
        }
        else
        {
            targetImage.sprite = DefaultSprite;
        }
    }
}
