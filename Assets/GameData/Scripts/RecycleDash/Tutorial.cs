using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Tutorial : MonoBehaviour
{
    private const string TutorialKey = "HasSeenTutorial";  // PlayerPrefs key to track if tutorial has been seen
    public PlayerController player;  // Reference to player controller
    public Image arrow;  // UI arrow image
    public Button playButton;   // Play button
    public TextMeshProUGUI textTutorial; // Tutorial text
    public GameObject goodJobMessage;  // Good job feedback message
    private Dictionary<string, ArrowPattern> patternsDict;  // Dictionary for arrow movement patterns
    [HideInInspector] public bool showTutorial;   // Flag to check if tutorial should be shown
    void Awake()
    {
        if (PlayerPrefs.GetInt(TutorialKey,0) == 0)
        {
            ShowTutorial();
            showTutorial = true;
        }
        else
        {
            showTutorial = false;
            gameObject.SetActive(false);
        }
        patternsDict = new Dictionary<string, ArrowPattern>
        {
            { "Up",  new ArrowPattern{ startPosition = new Vector2(0, -500), rotationZ = 0f, moveDistance = 1000f } },
            { "Down",    new ArrowPattern{ startPosition = new Vector2(0, 500),  rotationZ = 180f, moveDistance = 1000f } },
            { "Right", new ArrowPattern{ startPosition = new Vector2(300, 0),  rotationZ = 90f, moveDistance = 500f } },
            { "Left",  new ArrowPattern{ startPosition = new Vector2(-300,0),  rotationZ = -90f, moveDistance = 500f } },
        };
        if (x == 1)
        {
            StartCoroutine(DelayedSTart());
            x = 0;
        }
    }
    static int x = 0;

    // Activates the tutorial and saves the state
    void ShowTutorial()
    {
        gameObject.SetActive(true);
        PlayerPrefs.SetInt(TutorialKey, 1);
        PlayerPrefs.Save();
    }

    // Resets tutorial so it can be shown again

    public void ResetTutorial()
    {
        PlayerPrefs.SetInt(TutorialKey, 0);
        PlayerPrefs.Save();
        x = 1;
        SceneManager.LoadScene(2);
    }

    // Delays automatic clicking of play button
    IEnumerator DelayedSTart()
    {
        yield return new WaitForSeconds(0.25f);
        playButton.onClick.Invoke();
    }

    // Moves player back in the tutorial scene
    public void MoveBack()
    {
        Vector3 targetPos = player.transform.position - new Vector3(0, 0, 20);
        StartCoroutine(DelayedEx());
        player.transform.DOMove(targetPos, 1f).OnComplete(() => { player.run = true; }).SetDelay(2);
    }


    // Delayed actions for player during tutorial start
    IEnumerator DelayedEx()
    {
        yield return new WaitForSeconds(2.1f);
        player.animator.SetTrigger("Run");
        player.SetToNormalPos();
    }

    // Shows "Good Job" message with animation
    void GoodJobMessage(RectTransform target,Vector2 jumpTargetPos)
    {
        Vector2 initialPos = target.anchoredPosition;
        Vector3 initialScale = target.localScale;
        
        Sequence seq = DOTween.Sequence();
        seq.SetDelay(1.5f);
        seq.PrependCallback(() => target.gameObject.SetActive(true)).SetDelay(1.5f);
        seq.Append(target.DOAnchorPos(jumpTargetPos, 1, false));
        seq.Join(target.DOJumpAnchorPos(jumpTargetPos, 50f, 1, 1.5f));

        seq.Append(target.DOScale(initialScale * 1.2f, 0.2f));
        seq.Append(target.DOScale(Vector3.zero, 0.5f));

        seq.AppendCallback(() => target.gameObject.SetActive(false));
        seq.AppendCallback(() =>
        {
            target.anchoredPosition = initialPos;
            target.localScale = initialScale;
        });

    }
    // Shows arrow animation and sets tutorial text based on direction
    public void ShowArrow(string key)
    {
        if (!patternsDict.ContainsKey(key))
        {
            Debug.LogWarning($"No arrow pattern found for key: {key}");
            return;
        }
        if (key.Equals("Left"))
        {
            textTutorial.text = "Swipe Left";
            showTutorial = false;
            GoodJobMessage(goodJobMessage.GetComponent<RectTransform>(),new Vector2(0,400f));
        }
        else if (key.Equals("Down"))
        {
            textTutorial.text = "Swipe Down";
        }
        else if (key.Equals("Up"))
        {
            textTutorial.text = "Swipe up";
        }
        else if (key.Equals("Right"))
        {
            textTutorial.text = "Swipe right";
        }
        ArrowPattern pattern = patternsDict[key];

        // Apply position & rotation
        arrow.rectTransform.anchoredPosition = pattern.startPosition;
        arrow.rectTransform.rotation = Quaternion.Euler(0, 0, pattern.rotationZ);

        // Move & fade
        Vector2 targetPos = arrow.rectTransform.anchoredPosition + (Vector2)(arrow.rectTransform.up * pattern.moveDistance);
        MoveAndFade(targetPos, 1f, arrow);
    }

    // Moves arrow and fades text for tutorial feedback
    public void MoveAndFade(Vector3 targetPos, float moveDuration, Image image)
    {
        Vector3 initalPos = new Vector3(image.transform.position.x, image.transform.position.y, image.transform.position.z);
        Color txtColor = textTutorial.color;
        txtColor.a = 1f; // start fully visible
        textTutorial.color = txtColor;
        textTutorial.gameObject.SetActive(true);
        Color c = image.color;
        c.a = 0f;
        image.color = c;
        image.gameObject.SetActive(true);
        // Animate movement and fade
        Sequence seq = DOTween.Sequence();
        seq.Append(image.rectTransform.DOAnchorPos(targetPos, moveDuration).OnComplete(() => { image.transform.position = initalPos; }));
        seq.Join(image.DOFade(50f / 255f, moveDuration / 2f));
        seq.Join(image.DOFade(0f, moveDuration / 2f).SetDelay(moveDuration / 2f));
        seq.Join(textTutorial.DOFade(0f, moveDuration));
    }
}

// Struct to store arrow movement patterns
[System.Serializable]
public struct ArrowPattern
{
    public Vector2 startPosition;
    public float rotationZ;
    public float moveDistance;
}
