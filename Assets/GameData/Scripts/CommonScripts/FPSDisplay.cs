using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{

    /// <summary>
    /// // This script is just to view the fps while developing the game
    /// </summary>
    
    // Target Text for displaying fps
    public TextMeshProUGUI fpsText;
    private float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString() + " FPS";
    }
}
