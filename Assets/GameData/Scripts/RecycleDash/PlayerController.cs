using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player references")]
    GameObject activePlayer;
    public List<GameObject> players;
    public GameObject playerCamera;
    public GameObject pauseButton;
    public GameObject Score;
    [HideInInspector] public Animator animator;

    [Header("Movement parameters")]
    public float characterSpeed;
    public float laneTransitionSpeed = 0.3f;
    public float rotationEffect = 15f;
    public bool run = false;

    float middleLane = 0f;
    float leftLane = -1.25f;
    float rightLane = 1.25f;

    // PLayer positions
    public Enums.PlayerPosition playerPosition;

    private void Awake()
    {
        // Instantiate the selected character at a start position
        int character = PlayerPrefs.GetInt("selectedCharacter");
        if (character == 0)
        {

            activePlayer = Instantiate(players[0],transform);
            activePlayer.transform.localPosition = new Vector3(-3.5f, 0, -1.5f);
            activePlayer.transform.rotation = Quaternion.Euler(0, 120f, 0);
            animator = activePlayer.GetComponent<Animator>();
            
        }
        else
        {

            activePlayer = Instantiate(players[1],transform);
            activePlayer.transform.localPosition = new Vector3(-3.5f, 0, -1.5f);
            activePlayer.transform.rotation = Quaternion.Euler(0, 120f, 0);
            animator = activePlayer.GetComponent<Animator>();
         
        }
    }
    void Start()
    {
        // Subscribe to input events from InputManager
        playerPosition = Enums.PlayerPosition.center;
        InputManager.OnSwipeUp += Jump;
        InputManager.OnSwipeDown += SlideDown;
        InputManager.OnSwipeLeft += MoveLeft;
        InputManager.OnSwipeRight += MoveRight;
    }
    bool checkRotation = true;
    private void Update()
    {
        if (run)
        {
            // Move the player forward
            transform.Translate(Vector3.forward * characterSpeed * Time.deltaTime);
            // Reset local z and y to keep character upright on platform
            Vector3 pos = activePlayer.transform.position;
            pos.y = 0f;
            pos.z = 0f;
            activePlayer.transform.localPosition = pos;
            if (checkRotation)
            activePlayer.transform.rotation = Quaternion.identity;
        }
        // Optional: call keyboard() in Update if testing with keyboard or for PC builds
        //keyboard();
    }

    // Camera animation at the start
    public void MoveCamera()
    {
        playerCamera.transform.DORotate(new Vector3(15, 0, 0), 6f).SetEase(Ease.Linear).SetDelay(2);
        playerCamera.transform.DOLocalMove(new Vector3(0, 4, -5), 2f).SetDelay(8);
    }

    // Start the running animation and activate gameplay
    public void StartRunning()
    {
        animator.SetTrigger("Run");
        activePlayer.transform.DORotate(Vector3.zero, 2f);
        activePlayer.transform.DOLocalMove(Vector3.zero, 2f).OnComplete(() => { run = true; RunnerGameManager.Instance.playable = true; pauseButton.SetActive(true); Score.SetActive(true); });
    }

    // Reset player to default lane and rotation
    public void SetToNormalPos()
    {
        activePlayer.transform.localPosition= Vector3.zero;
        activePlayer.transform.rotation = Quaternion.identity;
        playerPosition = Enums.PlayerPosition.center;
        playerCamera.transform.DOMoveX(0, laneTransitionSpeed);

    }

    // Keyboard controls
    public void keyboard()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (playerPosition == Enums.PlayerPosition.center)
            {
                ChangeLane(rightLane, -rotationEffect - 10, -rotationEffect, 1f);
                playerPosition = Enums.PlayerPosition.right;
            }
            else if (playerPosition == Enums.PlayerPosition.left)
            {
                ChangeLane(middleLane, -rotationEffect - 10, -rotationEffect, 0f);
                playerPosition = Enums.PlayerPosition.center;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (playerPosition == Enums.PlayerPosition.center)
            {
                ChangeLane(leftLane, rotationEffect + 10, rotationEffect, -1f);
                playerPosition = Enums.PlayerPosition.left;
            }
            else if (playerPosition == Enums.PlayerPosition.right)
            {
                ChangeLane(middleLane, rotationEffect + 10, rotationEffect, 0f);
                playerPosition = Enums.PlayerPosition.center;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SlideDown();
        }
    }


    // Core lane change logic with rotation and camera movement
    public void ChangeLane(float position, float rotate, float faceRotation, float cameraPos)
    {
        checkRotation = false;
        playerCamera.transform.DOMoveX(cameraPos, laneTransitionSpeed);
        activePlayer.transform.DOMoveX(position, laneTransitionSpeed);
        activePlayer.transform.DORotate(new Vector3(0, faceRotation, rotate), laneTransitionSpeed / 2).OnComplete(
            () =>
            {
                activePlayer.transform.DORotate(new Vector3(0, 0, 0), laneTransitionSpeed / 2).OnComplete(() => { checkRotation = true; });
            });
    }

    #region Player actions
    void Jump()
    {
        animator.SetTrigger("Jump");
    }

    void SlideDown()
    {
        animator.SetTrigger("Slide");
    }
    void MoveLeft()
    {
        if (playerPosition == Enums.PlayerPosition.center)
        {
            ChangeLane(leftLane, rotationEffect + 10, rotationEffect, -0.5f);
            playerPosition = Enums.PlayerPosition.left;
        }
        else if (playerPosition == Enums.PlayerPosition.right)
        {
            ChangeLane(middleLane, rotationEffect + 10, rotationEffect, 0f);
            playerPosition = Enums.PlayerPosition.center;
        }
    }
    void MoveRight()
    {
        if (playerPosition == Enums.PlayerPosition.center)
        {
            ChangeLane(rightLane, -rotationEffect - 10, -rotationEffect, 0.5f);
            playerPosition = Enums.PlayerPosition.right;
        }
        else if (playerPosition == Enums.PlayerPosition.left)
        {
            ChangeLane(middleLane, -rotationEffect - 10, -rotationEffect, 0f);
            playerPosition = Enums.PlayerPosition.center;
        }
    }
    # endregion

    private void OnDestroy()
    {
        InputManager.OnSwipeUp -= Jump;
        InputManager.OnSwipeDown -= SlideDown;
        InputManager.OnSwipeLeft -= MoveLeft;
        InputManager.OnSwipeRight -= MoveRight;
    }



}
