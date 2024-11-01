using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    private CharacterController character;
    private Vector3 direction;
    public float gravity = 9.81f * 2;
    public float jumpForce = 8;
    public bool isJumping = false;

    public Animator animator;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        direction = Vector3.zero;
    }

    private void Update()
    {
        direction += Vector3.down * gravity * Time.deltaTime;
        if (character.isGrounded && GameManager.Instance.isSmall == false)
        {
            direction = Vector3.down;
            if (Input.GetButton("Jump") || Input.GetKey(KeyCode.W))
            {
                isJumping = true;
                direction = Vector3.up * jumpForce;
                animator.SetBool("isJumping", true);
            }
            else
            {
                isJumping = false;
                animator.SetBool("isJumping", false);
            }
        }

        character.Move(direction * Time.deltaTime);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
