
using DG.Tweening;
using UnityEngine;

public class ObjectCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Trash"))
        {
            RunnerGameManager.Instance.objectPickupManager.AddPickup(other.gameObject);
            AudioManager.Instance.PlaySound(9);
        }
        else if (other.gameObject.name.StartsWith("Slide"))
        {
            PlayerController player = RunnerGameManager.Instance.playerController;
            if (player != null)
            {
                AudioManager.Instance.PlaySound(11);
                player.run = false;
                RunnerGameManager.Instance.playable = false;
                player.animator.SetTrigger("FallBack");
                RunnerGameManager.Instance.uiManager.losePanel.SetActive(true);

            }
        }
        else if (other.gameObject.name.StartsWith("Hurdle"))
        {
            PlayerController player = RunnerGameManager.Instance.playerController;
            if (player != null)
            {
                AudioManager.Instance.PlaySound(11);
                player.run = false;
                RunnerGameManager.Instance.playable = false;
                player.animator.SetTrigger("FallBack");
                RunnerGameManager.Instance.uiManager.losePanel.SetActive(true);

            }
        }
        else if (other.gameObject.name.StartsWith("Factory")) 
        {
            PlayerController player = RunnerGameManager.Instance.playerController;
            player.gameObject.transform.DORotate(new Vector3(0, -90f, 0), 1f);
            player.characterSpeed = 5f;
            RunnerGameManager.Instance.SortItems();
            AudioManager.Instance.PlaySound(10);
        }


    }
}
