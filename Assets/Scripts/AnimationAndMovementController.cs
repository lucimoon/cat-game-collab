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
    [Tooltip("Distance before fall animation is triggered")]
    [SerializeField] private float fallHeight = 0.1f;

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
    Vector3 currentVerticalMovement = new Vector3();
    Vector3 currentMovement;
    bool isMovementPressed;
    bool isRunPressed;

    // Constants
    public float rotationFactorPerFrame = 15.0f;
    int zero = 0;
    float gravity;
    float groundedGravity = -.05f;

    // Jumping variables
    bool isJumpPressed = false;
    bool isJumping = false;
    bool isJumpAnimating = false;
    float previousHeight;

    void Awake()
    {
        initAnimatorReferences();
        initClassInstances();
        initComponentReferences();
        initPlayerInput();
    }

    void Update()
    {
        applyGravity();
        jump();
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

    private float getGravity()
    {
        float timeToFall = isFalling() ? jumpTimeToFall : jumpTimeToApex;

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

    private void animateMove()
    {
        animator.SetBool("isMoving", isMovementPressed);
        animator.SetBool("isRunning", isRunPressed);
    }

    private void rotate()
    {
        if (!isMovementPressed) return;

        Quaternion currentRotation = transform.rotation;
        Vector3 positionToLookAt = Camera.main.transform.TransformVector(currentMovement);
        positionToLookAt.y = zero;

        // Creates new rotation based on where the player is currently pressing
        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt.normalized, Vector3.up);
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
    }

    private void applyGravity()
    {
        animateFall();

        if (characterController.isGrounded)
        {
            if (isJumpAnimating)
            {
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
            }

            currentVerticalMovement.y = groundedGravity;
        }
        else
        {
            float previousVelocityY = currentVerticalMovement.y;
            float newVelocityY = currentVerticalMovement.y + (getGravity() * Time.deltaTime);
            float nextVelocityY = (previousVelocityY + newVelocityY) * 0.5f;

            currentVerticalMovement.y = nextVelocityY;
        }

        previousHeight = transform.position.y;
    }

    private bool isFalling()
    {
        return previousHeight - transform.position.y > fallHeight;
    }

    private void animateFall()
    {
        animator.SetBool(isFallingHash, isFalling());
    }

    private void jump()
    {
        bool shouldJump = !isJumping && characterController.isGrounded && isJumpPressed;
        bool hasLanded = isJumping && characterController.isGrounded && !isJumpPressed;

        if (shouldJump)
        {
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            currentVerticalMovement.y = getJumpVelocity();
        }
        else if (hasLanded)
        {
            isJumping = false;
        }
    }

    private void cancelJump()
    {
        currentVerticalMovement.y = 0;
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

        // apply jump/gravity
        cameraRelativeMotion += currentVerticalMovement;

        characterController.Move(cameraRelativeMotion * Time.deltaTime);

        rotate();
        animateMove();
    }
}
