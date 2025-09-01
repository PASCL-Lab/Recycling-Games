
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ObjectCollider : MonoBehaviour
{
    // Trigger logic when player collides with objects
    void OnTriggerEnter(Collider other)
    {
        // Tutorial objects
        if (other.gameObject.CompareTag("Tutorial"))
        {
            other.gameObject.GetComponent<Collider>().enabled = false;
            StartCoroutine(ActiveCollider(other.gameObject,3.5f));
            RunnerGameManager.Instance.tutorial.ShowArrow(other.gameObject.name);
        }
        // Slide or Hurdle during tutorial
        else if ((other.gameObject.CompareTag("Slide") || other.gameObject.CompareTag("Hurdle")) && RunnerGameManager.Instance.tutorial.showTutorial)
        {
            other.gameObject.GetComponent<Collider>().enabled = false;
            StartCoroutine(ActiveCollider(other.gameObject,1f));
            PlayerController player = RunnerGameManager.Instance.playerController;
            if (player != null)
            {
                AudioManager.Instance.PlaySound(11);
                player.run = false;
                player.animator.SetTrigger("FallBack");
                RunnerGameManager.Instance.tutorial.MoveBack();

            }
        }
        // Trash pickups
        else if (other.gameObject.CompareTag("Trash"))
        {
            RunnerGameManager.Instance.objectPickupManager.AddPickup(other.gameObject);
            AudioManager.Instance.PlaySound(9);
        }
        // Slide obstacle
        else if (other.gameObject.CompareTag("Slide"))
        {
            PlayerController player = RunnerGameManager.Instance.playerController;
            if (player != null)
            {
                AudioManager.Instance.PlaySound(11);
                player.run = false;
                player.animator.SetTrigger("FallBack");

                StartCoroutine(RunnerGameManager.Instance.ObstacleHit());

            }
        }
        // Hurdle obstacle
        else if (other.gameObject.CompareTag("Hurdle"))
        {
            PlayerController player = RunnerGameManager.Instance.playerController;
            if (player != null)
            {
                AudioManager.Instance.PlaySound(11);
                player.run = false;
                player.animator.SetTrigger("FallBack");
                StartCoroutine(RunnerGameManager.Instance.ObstacleHit());

            }
        }
        // Factory trigger: end of run, start sorting
        else if (other.gameObject.CompareTag("Factory")) 
        {
            other.gameObject.GetComponent<Collider>().enabled = false;
            PlayerController player = RunnerGameManager.Instance.playerController;
            player.run = false;
            player.gameObject.transform.DOMoveX(-30f, 8f);
            player.gameObject.transform.DORotate(new Vector3(0, -90f, 0), 0.8f);
            player.characterSpeed = 5f;
            RunnerGameManager.Instance.SortItems();
            RunnerGameManager.Instance.playable = false;
            AudioManager.Instance.PlaySound(10);
        }


    }
    // Re-enable collider after a delay
    IEnumerator ActiveCollider(GameObject other, float delay)
    {
        yield return new WaitForSeconds(delay);
        other.GetComponent<Collider>().enabled = true;
    }
}
