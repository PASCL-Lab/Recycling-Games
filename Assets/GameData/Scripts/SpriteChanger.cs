
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{
    public Image targetImage; // The Image component whose sprite you want to change
    public Sprite newSprite;
    public Sprite DefaultSprite;
    public Audiotype audioType;
    AudioSource audioSource;

    public enum Audiotype
    {
        SFX,
        BGMusic
    }

    private void Awake()
    {
        if (audioType == Audiotype.SFX)
        {
            audioSource = AudioManager.Instance.sfxSource;
        }
        else
        {
            audioSource = AudioManager.Instance.bgMusicSource;
        }
        CorrectSprite();
    }
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
