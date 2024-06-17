using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum state { idle, walking, running }
    public state activeState;
    public static GameManager Instance { get; private set; }
    public float initialGameSpeed = 0f;
    public float lastSpeed;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }    
    public bool firstSteps;
    
    public Animator birdAnim;
    public Animator animator;

    private Player player;
    private Spawner spawner;

    public UnityEngine.UI.Button retryButton;
    public UnityEngine.UI.Button resetButton;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;

    public float score;

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
        player = FindObjectOfType<Player>();
        spawner = FindObjectOfType<Spawner>();
        NewGame();
    }
    public void NewGame()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }
        activeState = state.idle;
        gameSpeed = initialGameSpeed;
        score = 0f;
        lastSpeed = 4f;
        firstSteps = true;
        animator.SetBool("isIdle", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);

        enabled = true;
        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(false);
        UpdateHiscore();
    }
    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;
        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        resetButton.gameObject.SetActive(true);
        UpdateHiscore();

    }
    private void Update()
    {
        score += gameSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
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
        
        if (player.isJumping == false)
        {
            switch (activeState)
            {
                case state.idle:
                    animator.speed = 1f;
                    birdAnim.speed = 1f;

                    break;
                case state.walking:
                    animator.speed = 0.5f * gameSpeed;
                    birdAnim.speed = 0.4f * gameSpeed;

                    break;
                case state.running:
                    animator.speed = 0.25f * gameSpeed;
                    birdAnim.speed = 0.25f * gameSpeed;
                    break;
            }
        }
        else
        {
            animator.speed = 1f;
            birdAnim.speed = 1f;
        }
        
    }
    private void UpdateHiscore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if(score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    }
    public void deleteScore()
    {
        score = 0f;
        PlayerPrefs.DeleteAll();
        UpdateHiscore();
    }
}
