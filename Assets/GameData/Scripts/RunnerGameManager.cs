
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunnerGameManager : MonoBehaviour
{
    public static RunnerGameManager Instance { get; private set; }
    public PlayerController playerController;
    public ObjectPickupManager objectPickupManager;
  
    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    public void Replay()
    {
        ItemManager.itemlist = objectPickupManager.pickups.ToList();
        SceneManager.LoadScene("SortingPhase");
        
        
    }
}
