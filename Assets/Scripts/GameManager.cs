using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    enum state { idle, walking, running }
    state activeState;
    public static GameManager Instance { get; private set; }
    public float initialGameSpeed = 0f;
    public float lastSpeed;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }


    public bool firstSteps;
    public Animator animator;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        NewGame();
    }
    private void NewGame()
    {
        activeState = state.idle;
        gameSpeed = initialGameSpeed;
        lastSpeed = 2.5f;
        firstSteps = true;
        animator.SetBool("isIdle", true);
    }
    private void Update()
    {
        if (activeState != state.idle)
        {
            gameSpeed += gameSpeedIncrease * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (activeState == state.idle)
            {
                activeState = state.walking;
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalking", true);
                if (firstSteps)
                {
                    gameSpeed = lastSpeed;
                    firstSteps = false;
                }
                else
                {
                    gameSpeed = lastSpeed;
                }
            }
            else if (activeState == state.walking)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);

                activeState = state.running;
                gameSpeed *= 1.5f;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (activeState == state.running)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);

                activeState = state.walking;
                gameSpeed /= 1.5f;
            }
            else if (activeState == state.walking)
            {
                animator.SetBool("isIdle", true);
                animator.SetBool("isWalking", false);

                activeState = state.idle;

                lastSpeed = gameSpeed;
                gameSpeed = 0;

            }
        }
    }
}
