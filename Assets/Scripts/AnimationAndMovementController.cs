using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class AnimationAndMovementController : MonoBehaviour
{

    // Declare reference variables
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;
    float speedMultiplier = 1f;

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
    float rotationFactorPerFrame = 15.0f;
    float walkMultiplier = 3.0f;
    float runMultiplier = 7.0f;
    int zero = 0;
    float gravity;
    float groundedGravity = -.05f;

    // Jumping variables
    public float maxJumpTime = 0.9f;
    public float maxJumpHeight = 6.0f;
    bool isJumpPressed = false;
    object isJumpPressedObject;
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
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
        speedMultiplier = isRunPressed ? runMultiplier : walkMultiplier;
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != zero || currentMovementInput.y != zero;
    }

    private float getGravity()
    {
        // TODO: serialize timeToFall so it's tweakable in editor
        float timeToFall = maxJumpTime / 2;
        return (-2 * maxJumpHeight) / Mathf.Pow(timeToFall, 2);
    }

    private float getJumpVelocity()
    {
        // TODO: serialize timeToApex so it's tweakable in editor
        float timeToApex = maxJumpTime / 2;
        return (2 * maxJumpHeight) / timeToApex;
    }

    private void handleAnimation()
    {
        animator.SetBool("isMoving", isMovementPressed);
        animator.SetBool("isRunning", isRunPressed);
    }

    private void handleRotation()
    {
        Vector3 positionToLookAt;
        // The change in position the character should point to
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = zero;
        positionToLookAt.z = currentMovement.z;

        // Current rotation of character
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            // Creates new rotation based on where the player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void handleGravity()
    {
        if (characterController.isGrounded)
        {
            if (isJumpAnimating)
            {
                animator.SetBool(isJumpingHash, false);
                animator.SetBool(isFallingHash, false);
                isJumpAnimating = false;
            }

            currentMovement.y = groundedGravity;
        }
        else
        {

            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (getGravity() * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentMovement.y = nextYVelocity;

            Debug.Log(nextYVelocity);
            if (previousYVelocity > 10 && nextYVelocity < 10)
            {
                animator.SetBool(isFallingHash, true);
            }
        }
    }

    private void handleJump()
    {
        bool shouldJump = !isJumping && characterController.isGrounded && isJumpPressed;
        bool hasLanded = !isJumpPressed && isJumping && characterController.isGrounded;

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
        Vector3 motion = new Vector3(
            currentMovement.x * speedMultiplier,
            currentMovement.y,
            currentMovement.z * speedMultiplier
        );

        characterController.Move(motion * Time.deltaTime);
    }
}
