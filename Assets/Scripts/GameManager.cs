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

    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }
    public float firstSpeed;

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
        firstSpeed = 2.5f;
        firstSteps = true;
    }
    private void Update()
    {
        if (activeState != state.idle)
        {
            gameSpeed += gameSpeedIncrease * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (activeState == state.idle)
            {
                activeState = state.walking;
                animator.SetBool("isWalking", true);
                if (firstSteps)
                {
                    gameSpeed = firstSpeed;
                    firstSteps = false;
                }
                else
                {
                    //gameSpeed = past speed
                }
            }
            else if (activeState == state.walking)
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
                activeState = state.running;
                gameSpeed *= 1.5f;
            }
        }
    }
}
