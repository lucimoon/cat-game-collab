using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  // Interface variables
  [Header("Movement Multipliers")]
  [SerializeField] private float walkMultiplier = 3.0f;
  [SerializeField] private float runMultiplier = 8.0f;
  [SerializeField] private float rotationFactorPerFrame = 15.0f;
  [SerializeField] private float pounceDistance = 1.2f;

  [Header("Jump Parameters")]
  [SerializeField] private float maxJumpHeight = 6.0f;
  [SerializeField] private float jumpTimeToApex = 0.45f;
  [SerializeField] private float jumpTimeToFall = 0.2f;
  [Tooltip("Distance before fall animation is triggered")]
  [SerializeField] private float fallHeight = 0.075f;

  [Header("Miscellaneous")]
  // Declare reference variables
  [SerializeField] private Transform mouthAttachmentPoint;
  private PlayerInput playerInput;
  private CharacterController characterController;
  private Animator animator;
  private Interactable interactable;

  // Variables to store optimized setter/getter parameter IDs
  private int isFallingHash;
  private int isJumpingHash;
  private int isMovingHash;
  private int isRunningHash;

  // Variables to store player input values
  private Vector3 currentMovementInput;
  private Vector3 currentVerticalMovement = new Vector3();
  private Vector3 currentMovement;
  private bool isMovementPressed;
  private bool isRunPressed;

  // Constants
  private int zero = 0;
  private float gravity;
  private float groundedGravity = -.05f;

  // Jumping variables
  private bool isJumpPressed = false;
  private bool isJumping = false;
  private bool isJumpAnimating = false;
  private float previousHeight;

  // Interact
  public bool isInteractPressed;
  InputAction interactAction;

  private PlayerAudio playerAudio;
  private float overrideSpeed;
  private float overrideRotationSpeed;
  private Vector3? overrideDestination;
  private Vector3? overrideRotation;
  private PlayerAnimation interactionAnimation;

  public Dictionary<string, string> interactionBindings = new Dictionary<string, string>();

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
    Move();
    interact();
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

    // Under interaction umbrella
    playerInput.CharacterControls.UsePaw.performed += onInteract;
    playerInput.CharacterControls.UsePaw.canceled += onInteract;
    interactionBindings.Add("UsePaw", playerInput.CharacterControls.UsePaw.GetBindingDisplayString(0));
    playerInput.CharacterControls.UseMouth.performed += onInteract;
    playerInput.CharacterControls.UseMouth.canceled += onInteract;
    interactionBindings.Add("UseMouth", playerInput.CharacterControls.UseMouth.GetBindingDisplayString(0));
    playerInput.CharacterControls.UseBody.performed += onInteract;
    playerInput.CharacterControls.UseBody.canceled += onInteract;
    interactionBindings.Add("UseBody", playerInput.CharacterControls.UseBody.GetBindingDisplayString(0));
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
    playerAudio = GetComponent<PlayerAudio>();
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

  void onInteract(InputAction.CallbackContext context)
  {
    interactAction = context.action;
    isInteractPressed = context.performed;
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
    animator.SetBool("isMoving", isMovementPressed || overrideDestination != null);
    animator.SetBool("isRunning", isRunPressed);
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
    // TODO: Replace ground checks with a raycasting solution
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

  private void interact()
  {
    bool shouldInteract = interactAction != null && interactAction.triggered && isInteractPressed;

    if (!shouldInteract) return;

    if (interactable != null)
    {
      UseObjectInteraction(interactAction.name);
      return;
    }

    UseDefaultReaction(interactAction.name);
  }

  // To-do: This should probably be updated to a method that tests proximity to an interactable
  void OnTriggerEnter(Collider other)
  {
    GameObject interactedObject = other.gameObject;

    if (interactedObject.CompareTag("Interactable"))
    {
      // Set our script reference to our newly collided interactable gameobject
      interactable = interactedObject.GetComponent<Interactable>();
      interactable.ShowTooltip();
    }
  }

  void OnTriggerExit()
  {
    // Note: This isn't triggered when Interactable in question is destroyed
    // Hide the interaction tool tip
    if (interactable != null) interactable.HideTooltip();
    // When we leave, set the current interactable references to null.
    interactable = null;
  }

  private void Move()
  {
    Vector3? motionVector;
    float speedMultiplier = getSpeedMultiplier();

    if (overrideDestination != null)
    {
      motionVector = CalculateOverrideMotion();
      transform.LookAt((Vector3)overrideDestination);
    }
    else
    {
      // Convert horizontal input to worldspace
      Vector3 cameraRelativeMotion = currentMovement;

      // apply horizontal speed multiplers
      cameraRelativeMotion.x *= speedMultiplier;
      cameraRelativeMotion.z *= speedMultiplier;

      // translate to camera relative direction
      cameraRelativeMotion = Camera.main.transform.TransformVector(cameraRelativeMotion);
      // apply jump/gravity
      cameraRelativeMotion += currentVerticalMovement;
      motionVector = cameraRelativeMotion * Time.deltaTime;
    }

    if (motionVector != null)
    {
      characterController.Move((Vector3)motionVector);
    }

    Rotate();
    animateMove();
  }

  private void Rotate()
  {
    Quaternion targetRotation;
    Quaternion currentRotation = transform.rotation;
    Vector3 lookTarget;
    float rotationSpeed = rotationFactorPerFrame;

    if (!isMovementPressed && overrideRotation == null) return;

    if (overrideRotation != null)
    {
      lookTarget = (Vector3)overrideRotation;
      rotationSpeed = overrideRotationSpeed;

      // translate target to rotation
      targetRotation = Quaternion.LookRotation(lookTarget.normalized, Vector3.up);


      float distance = Quaternion.Angle(transform.rotation, targetRotation);
      bool isAtDestination = distance < 1;
      Debug.Log(distance);
      Debug.Log(isAtDestination);

      if (isAtDestination)
      {
        overrideRotation = null;
        overrideRotationSpeed = default;
        return;
      }
      // set overrideRotation as target
    }
    else
    {
      // translate input to target
      lookTarget = Camera.main.transform.TransformVector(currentMovement);
      lookTarget.y = zero; // So we don't look up or down
      // translate target to rotation
      targetRotation = Quaternion.LookRotation(lookTarget.normalized, Vector3.up);
    }


    // rotate transform
    transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
  }

  private Vector3? CalculateOverrideMotion()
  {
    if (overrideDestination == null) return null;

    bool isAtDestination = Vector3.Distance(transform.position, (Vector3)overrideDestination) < 0.1f;
    if (isAtDestination)
    {
      overrideDestination = null;
      PlayAnimation(interactionAnimation);
      return null;
    }

    Vector3 targetPosition = Vector3.MoveTowards(transform.position, (Vector3)overrideDestination, overrideSpeed);
    return targetPosition - transform.position;
  }

  public void OverrideMovement(Vector3 targetPosition, float targetSpeed)
  {
    overrideDestination = targetPosition;
    this.overrideSpeed = targetSpeed;
  }

  public void OverrideRotation(Vector3 lookTarget, float rotationSpeed)
  {
    overrideRotation = lookTarget;
    overrideRotationSpeed = rotationSpeed;
  }

  public void OverrideRotation(float rotationSpeed)
  {
    overrideRotationSpeed = rotationSpeed;
  }

  public void OverrideRotation(Vector3 lookTarget)
  {
    overrideRotation = lookTarget;
  }

  private void UseObjectInteraction(string interactionName)
  {
    InteractionType interactionType = InteractionType.UseMouth;
    Transform interactionTransform = mouthAttachmentPoint;

    switch (interactionName)
    {
      case "UsePaw":
        interactionType = InteractionType.UsePaw;
        break;
      case "UseBody":
        interactionType = InteractionType.UseBody;
        interactionTransform = transform;
        break;
      default:
        break;
    }

    /*
    * This may be a weird pattern. We're
    * sneakily returning a PlayerAnimation from
    * a method that's called HandleInteraction.
    * Additionally it's being stored for later use when we
    * override the players movement. The walk animation
    * cancels this playback, then we attempt to play it
    * again when they arrive at the destination.
    */
    interactionAnimation = interactable.HandleInteraction(interactionType, interactionTransform);
    PlayAnimation(interactionAnimation);
  }

  private void UseDefaultReaction(string interactionName)
  {
    switch (interactionName)
    {
      case "UsePaw":
        UsePaw();
        break;
      case "UseBody":
        TakeRest();
        break;
      default:
        Meow();
        break;
    }
  }

  // Default behaviors

  private void Meow()
  {
    Debug.Log("Meow");
    // Default Animation and Audio lives here...
    animator.Play("Meow", -1);
    playerAudio.PlayMeow();
  }

  private void UsePaw()
  {
    Debug.Log("UsePaw");
    gameObject.transform.Translate(Vector3.forward * pounceDistance, Space.Self);
    animator.SetTrigger("Pounce");
  }

  private void TakeRest()
  {
    Debug.Log("TakeRest");
    animator.SetTrigger("Rest");
  }

  // End Default Behaviors

  private void PlayAnimation(PlayerAnimation animation)
  {
    // This can be considered complete when all animations in PlayerAnimation are added.
    switch (animation)
    {
      case PlayerAnimation.Nudge:
        animator.Play("Nudge", -1);
        break;
      case PlayerAnimation.Eat:
        animator.Play("Eat", -1);
        break;
      case PlayerAnimation.Pee:
        animator.Play("Pee", -1);
        break;
      default:
        break;
    }
  }

  // Animation helper
  public void TurnAround()
  {
    Vector3 lookTarget = -transform.forward;


    overrideRotation = lookTarget;
  }
}
