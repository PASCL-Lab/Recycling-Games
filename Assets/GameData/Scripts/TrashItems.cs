using DG.Tweening;
using UnityEngine;

public class TrashItems : MonoBehaviour
{
    public Enums.ItemType trashType;
    public int itemNumber;
    Vector3 offset;
    static bool check = true;
    [SerializeField] Collider itemCollider;
    [SerializeField] float returnJumpOffset;
    Camera mainCam;
    public Vector3 currentPos;
    Vector3 currentScale;
    private void Start()
    {
        currentScale = transform.localScale;
        mainCam = GameManager.Instance.mainCamera;
        check = true;
    }
    void OnMouseDown()
    {
        if (GameManager.Instance.gamePaused) return;
        offset = transform.position - MouseWorldPosition();
        transform.DOMoveZ(-9.4f, 0.15f);
        itemCollider.enabled = false;
        AudioManager.Instance.PlaySound(1);
    }
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
    Vector3 MouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = mainCam.WorldToScreenPoint(transform.position).z;
        return mainCam.ScreenToWorldPoint(mouseScreenPos);
    }
    public void MoveItem(TrashItems trash, TrashBins bin)
    {
        GameManager.Instance.scoreManager.IncrementScore(1);
        if (GameManager.Instance.trashBinManager.phase)
        {
            GameManager.Instance.pickUpManager.MoveNewItem(transform.parent);
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
