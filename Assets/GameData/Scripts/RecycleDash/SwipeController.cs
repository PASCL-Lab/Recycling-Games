using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

// Handles horizontal swipe or drag gestures to navigate through pages of the info panel
public class SwipeController : MonoBehaviour, IEndDragHandler
{
    
    [SerializeField] int maxPage; // Total number of pages
    int currentPage; // Current page index
    Vector3 targetPos; // Target local position of the pages container
    [Header("Amount of Movement for next page")]
    [SerializeField] Vector3 pageStep; // Amount to move for each page step
    [Header("Parent Rect of Pages")]
    [SerializeField] RectTransform levelPagesRect; // RectTransform that contains all pages
    [Header("Swipe Time")]
    [SerializeField] float tweenTime; // Duration of page movement animation
    float dragThreshold; // Minimum drag distance to trigger page change

    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
        dragThreshold = Screen.width / 15;
    }

    // Navigate to the next page if not at the last page
    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }
    // Navigate to the previous page if not at the first page
    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }
    // Animate the movement of the pages container to the target position
    public void MovePage()
    {
        levelPagesRect.DOLocalMove(targetPos, tweenTime).SetEase(Ease.OutCubic).SetUpdate(UpdateType.Normal, true);
    }

    // Called when the user finishes dragging (implements IEndDragHandler)
    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshold)
        {
            if (eventData.position.x > eventData.pressPosition.x) Previous();
            else Next();
        }
        else
        {
            MovePage(); 
        }
    }
}
