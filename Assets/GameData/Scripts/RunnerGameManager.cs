using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerGameManager : MonoBehaviour
{
    public static RunnerGameManager Instance { get; private set; }
    public ObjectPickupManager objectPickupManager;
  
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
}
