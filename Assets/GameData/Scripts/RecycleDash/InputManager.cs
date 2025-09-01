using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Touch positions for swipe detection
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 swipeDelta;

    // Minimum swipe distance to register a swipe
    [Header("Minimum finger movement in pixels to consider a swipe:")]
    public float swipeThreshold = 50f;

    // Events for swipe directions
    public delegate void SwipeAction();
    public static event SwipeAction OnSwipeUp;
    public static event SwipeAction OnSwipeDown;
    public static event SwipeAction OnSwipeLeft;
    public static event SwipeAction OnSwipeRight;


    void Update()
    {
        // Only detect touch if game is playable
        if (Input.touchCount > 0 && RunnerGameManager.Instance.playable)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position; // Record start of touch
            }

            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                endTouchPosition = touch.position; // Record end of touch
                swipeDelta = endTouchPosition - startTouchPosition; // Calculate swipe vector

                // Check if swipe distance exceeds threshold
                if (swipeDelta.magnitude > swipeThreshold)
                {
                    // Determine swipe direction and invoke events according to the direction of swipe
                    if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                    {
                        if (swipeDelta.x > 0)
                            OnSwipeRight?.Invoke();
                        else
                            OnSwipeLeft?.Invoke();
                    }
                    else
                    {
                        if (swipeDelta.y > 0)
                            OnSwipeUp?.Invoke();
                        else
                            OnSwipeDown?.Invoke();
                    }
                }

                ResetSwipe();// Clear swipe data
            }
        }
    }

    // Reset swipe tracking variables
    void ResetSwipe()
    {
        startTouchPosition = Vector2.zero;
        endTouchPosition = Vector2.zero;
        swipeDelta = Vector2.zero;
    }
}
