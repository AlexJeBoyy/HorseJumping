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
    public float walkSpeed;
    public float runSpeed;
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
        walkSpeed = 2.5f;
    }
    private void Update()
    {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (activeState == state.idle)
            {
                gameSpeed = walkSpeed;
            }
        }
    }
}
