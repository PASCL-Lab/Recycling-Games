using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    public TextMeshProUGUI textTimer; // UI text component to display the timer
    public bool timerStart = false;  // Flag to start/stop the timer
    public float timevalue = 120f;  // Current time value in seconds
    public Camera cam;  // Reference to camera for shake effect
    bool done = true; // Flag to trigger the red color & camera shake only once

    private const float BaseTime = 180f;
    private const float TimePerLevel = 60f;

    // Returns the total time for a given level
    public static float GetTimeForLevel(int level)
    {
        return BaseTime + (level - 1) * TimePerLevel;
    }
    private void Start()
    {
        int level = GameManager.Instance.GetLevel();
        timevalue = GetTimeForLevel(level);
        DispayTimer(timevalue);
    }
    private void Update()
    {
        if (timerStart)
        {
            if (timevalue > 0)
            {
                if (timevalue < 60 && done)
                {
                    textTimer.color = Color.red;
                    StartCoroutine(ShakeCamera());
                    done = false;
                }
                DispayTimer(timevalue);
                timevalue -= Time.deltaTime;
            }
            else
            {
                EndTimer();
            }
        }
    }

    // Coroutine to shake camera briefly when timer is low
    IEnumerator ShakeCamera()
    {
        AudioManager.Instance.PlaySound(4);
        Vector3 originalpos = cam.transform.position;
        float elapsedTime = 0;
        while (elapsedTime < 0.2f)
        {
            elapsedTime += Time.deltaTime;
            float x = Random.Range(0.05f, -0.05f);
            float y = Random.Range(0.05f, -0.05f);
            cam.transform.position = new Vector3(originalpos.x-x,originalpos.y- y, -5);
            yield return null;
        }
        cam.transform.position = originalpos;
    }

    // Called when timer reaches 0
    void EndTimer()
    {
        timerStart = false;
        GameManager.Instance.SortFailed();
    }

    // Display timer in MM:SS format
    void DispayTimer(float TimetoDisplay)
    {
        float minutes = Mathf.FloorToInt(TimetoDisplay / 60);
        float seconds = Mathf.FloorToInt(TimetoDisplay % 60);
        if (seconds >= 10)
        {
            textTimer.text = minutes + ":" + seconds;
        }
        else { textTimer.text =  minutes + ":0" + seconds; }
    }
    
}