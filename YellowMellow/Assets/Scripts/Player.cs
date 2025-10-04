using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float acceleration = 20f;
    public float maxSpeed = 10f;
    public float verticalSpeed = 10f;
    public float damping = 5f; // How quickly the player slows down

    [Header("References")]
    public Camera playerCamera;

    private CharacterController controller;
    private PlayerInputActions inputActions;

    private Vector2 moveInput;
    private Rigidbody rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        inputActions = new PlayerInputActions();
        controller = GetComponent<CharacterController>();

        // Movement input
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector3 inputDirection = transform.right * moveInput.x;// + transform.forward * moveInput.y;

        if (moveInput.y > 0)
            inputDirection += Vector3.up;
        else
            inputDirection += Vector3.down;

        // Target velocity based on input
        Vector3 targetVelocity = inputDirection * maxSpeed;

        // Calculate force needed to reach target velocity
        Vector3 velocityDiff = targetVelocity - rb.linearVelocity;
        Vector3 accelerationForce = velocityDiff * acceleration;

        // Apply force (ignores mass to feel snappy)
        rb.AddForce(accelerationForce, ForceMode.Acceleration);
        // Apply damping to reduce unwanted drift (simulate friction)
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, damping * Time.fixedDeltaTime);
    }
}
