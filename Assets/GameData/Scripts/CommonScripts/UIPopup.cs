using UnityEngine;
using DG.Tweening;

public class UIPopup : MonoBehaviour
{
    [Header("Animation Settings")]
    public float popupDuration = 0.4f; // Duration for the pop-in animation
    public float hideDuration = 0.3f;  // Duration for the hide animation
    public Ease popupEase = Ease.OutBack; // Ease type for pop-in
    public Ease hideEase = Ease.InBack;   // Ease type for hide

    private Vector3 originalScale;  // Stores the original scale of the UI element
    private Tween currentTween;   // Reference to the current tween to safely kill it before creating a new one

    private void Awake()
    {
        // Store the initial scale and set the object to zero for hidden state
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        // Automatically play popup animation when the object becomes active
        PlayPopup();
    }

    public void PlayPopup()
    {
        // Kill any existing tween to prevent conflicts
        currentTween?.Kill();

        // Reset scale and tween to the original scale
        transform.localScale = Vector3.zero;
        currentTween = transform.DOScale(originalScale, popupDuration)
            .SetEase(popupEase).SetUpdate(true);
    }

    // Animate the UI element hiding
    public void HidePopup()
    {
        currentTween?.Kill();
        // Tween to zero scale and deactivate the object once complete
        currentTween = transform.DOScale(Vector3.zero, hideDuration)
            .SetEase(hideEase).SetUpdate(true)
            .OnComplete(() => gameObject.SetActive(false));
    }
}
