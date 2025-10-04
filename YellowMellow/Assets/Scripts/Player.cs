using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;


public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float acceleration = 20f;
    public float maxSpeed = 10f;
    public float verticalSpeed = 10f;
    public float damping = 5f; // How quickly the player slows down
    public float naturalDrag = 0.1f;   // Light resistance (set to 0 for perfect space)
    public float gravityCompensation = 9.8f * 0.2f; // 80% counter to gravity
    public static bool playerPaused = false;

    [Header("References")]
    public Camera playerCamera;

    private PlayerInputActions inputActions;

    private Vector2 moveInput;
    private Rigidbody rb;

    public GameObject StolenItem;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        inputActions = new PlayerInputActions();

        // Movement input
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();
   
    public static void SetGamePaused(bool paused)
    {
        playerPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
    }

    void FixedUpdate()
    {
        if (playerPaused) {
            rb.Sleep();
            return;
        };
        HandleMovement();
        ApplyDrag();
        //rb.AddForce(Vector3.up * gravityCompensation, ForceMode.Acceleration);
        HandleSpriteFlip();
        if (StolenItem != null)
        {
            ReleaseItem();
        }

    }

    void HandleMovement()
    {
        Vector3 inputDirection = transform.right * moveInput.x + Vector3.up * moveInput.y;
        inputDirection = inputDirection.normalized;



        // Only apply force if there's input
        if (inputDirection.sqrMagnitude > 0.01f)
        {
            rb.AddForce(inputDirection * acceleration, ForceMode.Acceleration);
        }

        // Apply damping to reduce unwanted drift (simulate friction)
        if (moveInput.sqrMagnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity * maxSpeed;
    }


    void ApplyDrag()
    {
        // Light drag over time
        rb.linearVelocity *= (1f - naturalDrag * Time.fixedDeltaTime);
    }
    void HandleSpriteFlip()
    {
        if (moveInput.x < -0.01f)
        {
            // Moving left
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
        else if (moveInput.x > 0.01f)
        {
            // Moving right
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
    void ReleaseItem()
    {
        if (!playerPaused)
        {
            // Trigger when space is released
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Instantiate(StolenItem, this.transform);
            }
        }
    }

}
