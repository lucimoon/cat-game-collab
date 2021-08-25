using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class AnimationAndMovementController : MonoBehaviour
{
    // Interface variables
    [Header("Speed Multipliers")]
    [SerializeField] private float walkMultiplier = 3.0f;
    [SerializeField] private float runMultiplier = 7.0f;

    [Header("Jump Parameters")]
    [SerializeField] private float maxJumpHeight = 6.0f;
    [SerializeField] private float jumpTimeToApex = 0.45f;
    [SerializeField] private float jumpTimeToFall = 0.2f;

    // Declare reference variables
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    // Variables to store optimized setter/getter parameter IDs
    int isFallingHash;
    int isJumpingHash;
    int isMovingHash;
    int isRunningHash;

    // Variables to store player input values
    Vector3 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;
    bool isRunPressed;

    // Constants
    public float rotationFactorPerFrame = 15.0f;
    int zero = 0;
    float gravity;
    float groundedGravity = -.05f;
    float FALL_THRESHOLD = -2f;

    // Jumping variables

    bool isJumpPressed = false;
    bool isJumping = false;
    bool isJumpAnimating = false;

    void Awake()
    {
        initAnimatorReferences();
        initClassInstances();
        initComponentReferences();
        initPlayerInput();
    }

    void Update()
    {
        handleAnimation();
        handleRotation();
        handleGravity();
        handleJump();
        move();
    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

    void initPlayerInput()
    {
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;
    }

    void initAnimatorReferences()
    {
        isMovingHash = Animator.StringToHash("isMoving");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        isFallingHash = Animator.StringToHash("isFalling");
    }

    void initComponentReferences()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void initClassInstances()
    {
        playerInput = new PlayerInput();
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();

        if (context.phase == InputActionPhase.Canceled) cancelJump();
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != zero || currentMovementInput.y != zero;
    }

    private float getGravity(bool isFalling = false)
    {
        float timeToFall = isFalling ? jumpTimeToFall : jumpTimeToApex;

        return (-2 * maxJumpHeight) / Mathf.Pow(timeToFall, 2);
    }

    private float getJumpVelocity()
    {
        return (2 * maxJumpHeight) / jumpTimeToApex;
    }

    private float getSpeedMultiplier()
    {
        return isRunPressed ? runMultiplier : walkMultiplier;
    }

    private void cancelJump()
    {
        currentMovement.y = 0;
        animator.SetBool(isFallingHash, true);
    }

    private void handleAnimation()
    {
        animator.SetBool("isMoving", isMovementPressed);
        animator.SetBool("isRunning", isRunPressed);
    }

    private void handleRotation()
    {
        Quaternion currentRotation = transform.rotation;
        Vector3 positionToLookAt = Camera.main.transform.TransformVector(currentMovement);
        // The change in position the character should point to
        positionToLookAt.y = zero;


        if (isMovementPressed)
        {
            // Creates new rotation based on where the player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt.normalized, Vector3.up);
            transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void handleGravity()
    {
        if (characterController.isGrounded)
        {
            animator.SetBool(isFallingHash, false);
            if (isJumpAnimating)
            {
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
            }

            currentMovement.y = groundedGravity;
        }
        else
        {
            float gravity;
            float previousVelocityY = currentMovement.y;

            if (previousVelocityY < FALL_THRESHOLD)
            {
                gravity = getGravity(true);
                animator.SetBool(isFallingHash, true);
            }
            else
            {
                gravity = getGravity();
            }

            float newVelocityY = currentMovement.y + (getGravity() * Time.deltaTime);
            float nextVelocityY = (previousVelocityY + newVelocityY) * 0.5f;
            currentMovement.y = nextVelocityY;
        }
    }

    private void handleJump()
    {
        bool shouldJump = !isJumping && characterController.isGrounded && isJumpPressed;
        bool hasLanded = isJumping && characterController.isGrounded && !isJumpPressed;

        if (shouldJump)
        {
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            currentMovement.y = getJumpVelocity();
        }
        else if (hasLanded)
        {
            isJumping = false;
        }
    }

    private void move()
    {
        float speedMultiplier = getSpeedMultiplier();

        // Convert horizontal input to worldspace
        Vector3 cameraRelativeMotion = currentMovement;

        // apply horizontal speed multiplers
        cameraRelativeMotion.x *= speedMultiplier;
        cameraRelativeMotion.z *= speedMultiplier;

        // translate to camera relative direction
        cameraRelativeMotion = Camera.main.transform.TransformVector(cameraRelativeMotion);

        // restore original gravity value
        cameraRelativeMotion.y = currentMovement.y;

        // Move
        characterController.Move(cameraRelativeMotion * Time.deltaTime);

    }
}
