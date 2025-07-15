using UnityEngine;
using DG.Tweening;

public class UIPopup : MonoBehaviour
{
    [Header("Animation Settings")]
    public float popupDuration = 0.4f;
    public float hideDuration = 0.3f;
    public Ease popupEase = Ease.OutBack;
    public Ease hideEase = Ease.InBack;

    private Vector3 originalScale;
    private Tween currentTween;

    private void Awake()
    {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        PlayPopup();
    }

    public void PlayPopup()
    {
        currentTween?.Kill();

        transform.localScale = Vector3.zero;
        currentTween = transform.DOScale(originalScale, popupDuration)
            .SetEase(popupEase).SetUpdate(true);
    }

    public void HidePopup()
    {
        currentTween?.Kill();

        currentTween = transform.DOScale(Vector3.zero, hideDuration)
            .SetEase(hideEase).SetUpdate(true)
            .OnComplete(() => gameObject.SetActive(false));
    }
}
