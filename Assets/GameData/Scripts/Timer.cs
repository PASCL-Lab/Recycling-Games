using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    public TextMeshProUGUI textTimer;
    public bool timerStart = false;
    public float timevalue = 120f;
    public Camera cam;
    bool done = true;
    private void Start()
    {
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
    void EndTimer()
    {
        timerStart = false;
        GameManager.Instance.SortFailed();
    }
    void DispayTimer(float TimetoDisplay)
    {
        float minutes = Mathf.FloorToInt(TimetoDisplay / 60);
        float seconds = Mathf.FloorToInt(TimetoDisplay % 60);
        if (seconds >= 10)
        {
            textTimer.text = "0" + minutes + ":" + seconds;
        }
        else { textTimer.text = "0" + minutes + ":0" + seconds; }
    }
    
}