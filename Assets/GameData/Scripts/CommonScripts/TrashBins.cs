using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class TrashBins : MonoBehaviour
{
    public Enums.ItemType binType; // Type of trash this bin holds
    public int capacity;  // Maximum number of items this bin can hold
    public Transform Lid; // Transform of the bin lid for opening/closing
    public Transform trashPoint;  // Position where trash items are thrown into the bin
    public TextMeshProUGUI text; // UI text displaying the bin capacity

    // Coroutine to open the bin with animation
    public IEnumerator OpenBin()
    {
        yield return new WaitForSeconds(0.7f);
        AudioManager.Instance.PlaySound(6);
        transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f)
            .OnComplete(() =>
            {
                Lid.transform.DOLocalRotate(new Vector3(-100f, 0, 0), 0.2f).SetEase(Ease.OutBounce).OnComplete(() => { transform.GetComponent<Collider>().enabled = true; });
            });

    }

    // Close the bin with animation
    public void CloseBin()
    {
        Lid.transform.DOLocalRotate(new Vector3(0f, 0, 0), 0.2f);
        AudioManager.Instance.PlaySound(5);
        transform.DOLocalRotate(new Vector3(0, -90f, 0), 0.2f).OnComplete(() => { transform.DOLocalMoveX(-5f, 1f); });
    }
}
