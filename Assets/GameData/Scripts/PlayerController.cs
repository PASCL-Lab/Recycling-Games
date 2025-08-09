using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameObject activePlayer;
    public List<GameObject> players;
    public GameObject playerCamera;
    [HideInInspector] public Animator animator;
    public float characterSpeed;
    public float laneTransitionSpeed = 0.3f;
    public float rotationEffect = 15f;
    public bool run = false;

    float middleLane = 0f;
    float leftLane = -1.25f;
    float rightLane = 1.25f;


    public Enums.PlayerPosition playerPosition;
    public Enums.PlayerState playerState;

    private void Awake()
    {
        int character = PlayerPrefs.GetInt("selectedCharacter");
        if (character == 0)
        {

            activePlayer = Instantiate(players[0], transform);
            animator = activePlayer.GetComponent<Animator>();
            
        }
        else
        {

            activePlayer = Instantiate(players[1], transform);
            animator = activePlayer.GetComponent<Animator>();
         
        }
    }
    void Start()
    {

        playerPosition = Enums.PlayerPosition.center;
        playerState = Enums.PlayerState.running;
        InputManager.OnSwipeUp += Jump;
        InputManager.OnSwipeDown += SlideDown;
        InputManager.OnSwipeLeft += MoveLeft;
        InputManager.OnSwipeRight += MoveRight;

    }

    private void Update()
    {
        if (run)
            transform.Translate(Vector3.forward * characterSpeed * Time.deltaTime);
    }

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


    public void ChangeLane(float position, float rotate, float faceRotation, float cameraPos)
    {
        playerCamera.transform.DOMoveX(cameraPos, laneTransitionSpeed);
        activePlayer.transform.DOMoveX(position, laneTransitionSpeed);
        activePlayer.transform.DORotate(new Vector3(0, faceRotation, rotate), laneTransitionSpeed / 2).OnComplete(
            () =>
            {
                activePlayer.transform.DORotate(new Vector3(0, 0, 0), laneTransitionSpeed / 2);
            });
    }

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

    private void OnDestroy()
    {
        InputManager.OnSwipeUp -= Jump;
        InputManager.OnSwipeDown -= SlideDown;
        InputManager.OnSwipeLeft -= MoveLeft;
        InputManager.OnSwipeRight -= MoveRight;
    }



}
