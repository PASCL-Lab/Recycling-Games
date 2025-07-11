using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 swipeDelta;

    public float swipeThreshold = 50f;

    public delegate void SwipeAction();
    public static event SwipeAction OnSwipeUp;
    public static event SwipeAction OnSwipeDown;
    public static event SwipeAction OnSwipeLeft;
    public static event SwipeAction OnSwipeRight;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }

            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                endTouchPosition = touch.position;
                swipeDelta = endTouchPosition - startTouchPosition;

                if (swipeDelta.magnitude > swipeThreshold)
                {
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

                ResetSwipe();
            }
        }
    }

    void ResetSwipe()
    {
        startTouchPosition = Vector2.zero;
        endTouchPosition = Vector2.zero;
        swipeDelta = Vector2.zero;
    }
}
