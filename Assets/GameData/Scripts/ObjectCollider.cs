
using UnityEngine;

public class ObjectCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Trash"))
        {
            RunnerGameManager.Instance.objectPickupManager.AddPickup(other.gameObject);
        }
        else if (other.gameObject.name.StartsWith("Slide"))
        {
            PlayerController player = RunnerGameManager.Instance.playerController;
            if (player != null)
            {
                
                    player.run = false;
                    player.animator.SetTrigger("FallBack");
                
            }
        }
        else if (other.gameObject.name.StartsWith("Hurdle"))
        {
            PlayerController player = RunnerGameManager.Instance.playerController;
            if (player != null)
            {
                
                    player.run = false;
                    player.animator.SetTrigger("FallBack");
                
            }
        }
        

    }
}
