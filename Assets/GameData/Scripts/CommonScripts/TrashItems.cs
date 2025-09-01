using DG.Tweening;
using UnityEngine;

public class TrashItems : MonoBehaviour
{
    public Enums.ItemType trashType; // Type of this trash item
    public int itemNumber; // Unique number/id for the trash item
    Vector3 offset;   // Offset between mouse position and item position during drag
    static bool check = true;  // Used to start timer only once on first drag
    [SerializeField] Collider itemCollider;  // Collider to enable/disable during drag
    Camera mainCam;
    public Vector3 currentPos;  // Starting position of the item
    Vector3 currentScale;   // Original scale of the item
    private void Start()
    {
        currentScale = transform.localScale;
        mainCam = GameManager.Instance.mainCamera;
        check = true;
    }

    // Called when player clicks on the item
    void OnMouseDown()
    {
        if (GameManager.Instance.gamePaused) return;
        offset = transform.position - MouseWorldPosition();
        transform.DOMoveZ(-9.4f, 0.15f);
        itemCollider.enabled = false;
        AudioManager.Instance.PlaySound(1);
    }

    // Called while the player drags the item
    void OnMouseDrag()
    {
        if (GameManager.Instance.gamePaused) return;
        Vector3 newPos = MouseWorldPosition() + offset;
        //newPos = new Vector3(newPos.x, newPos.y,-9.4f);
        transform.position = newPos;

        //Timer Code
        if (check)
        {
            GameManager.Instance.timer.timerStart = true;
            check = false;
        }
    }

    // Called when player releases the mouse button
    void OnMouseUp()
    {
        if (GameManager.Instance.gamePaused) return;
        Vector3 rayOrigin = mainCam.transform.position;
        Vector3 rayDirection = MouseWorldPosition() - mainCam.transform.position;
        RaycastHit hitInfo;
        if (Physics.Raycast(rayOrigin, rayDirection * 200f, out hitInfo))
        {
            TrashBins bin = hitInfo.transform.gameObject.GetComponent<TrashBins>();
            if (bin != null && this.trashType == bin.binType && bin.capacity!=0)
            {
                MoveItem(this, bin);
            }
            else
            {
                ReturnBack();
            }
        }
        else
        {
            ReturnBack();
        }
        itemCollider.enabled = true;
    }

    // Converts mouse screen position to world position
    Vector3 MouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = mainCam.WorldToScreenPoint(transform.position).z;
        return mainCam.ScreenToWorldPoint(mouseScreenPos);
    }

    // Move the item into the correct bin with animation
    public void MoveItem(TrashItems trash, TrashBins bin)
    {
        GameManager.Instance.scoreManager.IncrementScore(1);
        if (GameManager.Instance.trashBinManager.phase)
        {
            GameManager.Instance.garbageItemManager.MoveNewItem(transform.parent);
        }
        else
        {
            GameManager.Instance.itemManager.MoveNewItem(transform.parent);
        }
        trash.transform.parent = bin.trashPoint;
        trash.transform.DOLocalMove(Vector3.zero, 0.15f);
        trash.transform.DOScale(0, 0.2f);
        PinchDown(bin.transform, bin.transform.localScale);
        AudioManager.Instance.PlaySound(3);
        bin.capacity -= 1;
        bin.text.text = bin.capacity.ToString();
        if (bin.capacity == 0)
        {
            GameManager.Instance.trashBinManager.RemoveBin(bin);
        }
    }

    // Returns the item back to its original position if placed incorrectly
    void ReturnBack()
    {
        GameManager.Instance.scoreManager.DecrementScore(1);
        Vector3 targetpos = currentPos;
        transform.DOLocalMove(targetpos, 0.15f)
            .OnComplete(() =>
            {
                PinchDown(transform, currentScale);
                AudioManager.Instance.PlaySound(2);
            });
    }

    // Pinch animation effect for items/bins
    public void PinchDown(Transform targetTransform, Vector3 curScale)
    {
        Sequence pinchSeq = DOTween.Sequence();
        pinchSeq.Append(targetTransform.DOScaleY(targetTransform.localScale.y - 0.1f, 0.1f));
        pinchSeq.Join(targetTransform.DOScaleX(targetTransform.localScale.x + 0.15f, 0.1f));
        pinchSeq.Append(targetTransform.DOScaleY(targetTransform.localScale.y, 0.1f));
        pinchSeq.Join(targetTransform.DOScaleX(targetTransform.localScale.x, 0.1f));
        pinchSeq.OnComplete(() => { targetTransform.localScale = curScale; });
    }

}
