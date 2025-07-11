using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class TrashBins : MonoBehaviour
{
    public Enums.ItemType binType;
    public int capacity;
    public Transform Lid;
    public Transform trashPoint;
    public TextMeshProUGUI text;
    public IEnumerator OpenBin()
    {
        yield return new WaitForSeconds(1.5f);
        transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f)
            .OnComplete(() =>
            {
                Lid.transform.DOLocalRotate(new Vector3(-100f, 0, 0), 0.2f).OnComplete(() => { transform.GetComponent<Collider>().enabled = true; });
            });

    }
    public void CloseBin()
    {
        Lid.transform.DOLocalRotate(new Vector3(0f, 0, 0), 0.2f).OnComplete(() => {
            transform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f).OnComplete(() => { transform.DOLocalMoveX(-5f,1f); });
        });
    }
}
