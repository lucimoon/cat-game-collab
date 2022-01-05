using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  // Interface variables
  [Header("Player Input Multipliers")]
  [Tooltip("Walk speed for player input")]
  [SerializeField] private float walkMultiplier = 3.0f;
  [Tooltip("Run speed for player input")]
  [SerializeField] private float runMultiplier = 8.0f;
  [Tooltip("Turn speed for player input")]
  [SerializeField] private float rotationFactorPerFrame = 15.0f;
  [Tooltip("Distance player pounces")]
  [SerializeField] private float pounceDistance = 1.2f;

  [Header("Jump Parameters")]
  [Tooltip("How high the player can jump")]
  [SerializeField] private float maxJumpHeight = 6.0f;
  [Tooltip("How long the player ascends")]
  [SerializeField] private float jumpTimeToApex = 0.45f;
  [Tooltip("How long the player descends")]
  [SerializeField] private float jumpTimeToFall = 0.2f;
  [Tooltip("Distance before fall animation is triggered")]
  [SerializeField] private float fallHeight = 0.075f;

  [Header("Miscellaneous")]
  // Declare reference variables
  [Tooltip("Where held items attach to player's mouth")]
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
  public Vector3 currentMovement;
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

  // Input Overrides
  private float overrideSpeed;
  private float overrideRotationSpeed;
  private Vector3? overrideDestination = null;
  private Vector3? overrideRotation = null;

  private PlayerAudio playerAudio;
  private PlayerAnimation interactionAnimation;

  public Dictionary<string, string> interactionBindings = new Dictionary<string, string>();

  // Services
  public IUnityService UnityService;
  public float test = 0f;

  void Awake()
  {
    initAnimatorReferences();
    initComponentReferences();
    if (UnityService == null) UnityService = new UnityService();
  }

  void Update()
  {
    applyGravity();
    jump();
    Move();
    interact();
  }

  private void initAnimatorReferences()
  {
    isMovingHash = Animator.StringToHash("isMoving");
    isRunningHash = Animator.StringToHash("isRunning");
    isJumpingHash = Animator.StringToHash("isJumping");
    isFallingHash = Animator.StringToHash("isFalling");
  }

  private void initComponentReferences()
  {
    characterController = GetComponent<CharacterController>();
    animator = GetComponent<Animator>();
    playerAudio = GetComponent<PlayerAudio>();
  }

  public void onJump(InputAction.CallbackContext context)
  {
    isJumpPressed = context.ReadValueAsButton();
    if (context.phase == InputActionPhase.Canceled) cancelJump();
  }

  public void onRun(InputAction.CallbackContext context)
  {
    isRunPressed = context.ReadValueAsButton();
  }

  public void onInteract(InputAction.CallbackContext context)
  {
    interactAction = context.action;
    isInteractPressed = context.performed;
  }

  public void onMovementInput(InputAction.CallbackContext context)
  {
    currentMovementInput = context.ReadValue<Vector2>();
    currentMovement.x = currentMovementInput.x;
    currentMovement.z = currentMovementInput.y;
    isMovementPressed = currentMovementInput.x != zero || currentMovementInput.y != zero;
  }

  private void animateMove()
  {
    animator.SetBool("isMoving", this.IsPlayerMoving);
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
      float newVelocityY = currentVerticalMovement.y + (this.Gravity * UnityService.GetDeltaTime());
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
      currentVerticalMovement.y = this.JumpVelocity;
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

  void OnTriggerExit(Collider otherCollider)
  {
    if (otherCollider.tag == "Interactable")
    {
      // Note: This isn't triggered when Interactable in question is destroyed
      // When we leave, set the current interactable references to null.
      interactable = null;
    }
  }

  private void Move()
  {
    Translate();
    Rotate();
    animateMove();
  }

  private void Translate()
  {
    Vector3? motionVector;

    if (overrideDestination != null)
    {
      transform.LookAt((Vector3)overrideDestination);
      motionVector = this.OverrideToMotion;
    }
    else
    {
      motionVector = this.InputToMotion;
    }

    if (motionVector == null) return;

    characterController.Move((Vector3)motionVector);
  }

  private void Rotate()
  {
    if (!this.IsPlayerTurning) return;

    float rotationSpeed = rotationFactorPerFrame;
    Quaternion currentRotation = transform.rotation;
    Quaternion targetRotation;

    if (overrideRotation != null)
    {
      rotationSpeed = overrideRotationSpeed;
      targetRotation = this.OverrideTargetRotation;
      ClearOverrideWhenDone(targetRotation);
    }
    else
    {
      targetRotation = this.InputTargetRotation;
    }

    transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, rotationSpeed * UnityService.GetDeltaTime());
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
        interactionTransform = transform;
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
        Pounce();
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
    // Default Animation and Audio lives here...
    animator.Play("Meow", -1);
    playerAudio.PlayMeow();
  }

  private void Pounce()
  {
    gameObject.transform.Translate(Vector3.forward * pounceDistance, Space.Self);
    animator.SetTrigger("Pounce");
  }

  private void TakeRest()
  {
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
      case PlayerAnimation.Scratch:
        animator.Play("Scratch", -1);
        break;
      default:
        Debug.Log("No animation configured in PlayerController");
        break;
    }
  }

  // Allows animation event to trigger a 180 Turn
  public void TurnAround()
  {
    overrideRotation = -transform.forward;
  }


  /**
   * Utility methods below
   * Collecting these simple getters below.
   * They can be moved to a helper class if the mood strikes.
   */

  private bool IsPlayerMoving
  {
    get
    {
      return isMovementPressed || overrideDestination.HasValue;
    }
  }

  private bool IsPlayerTurning
  {
    get
    {
      return isMovementPressed || overrideRotation != null;
    }
  }

  private float SpeedMultiplier
  {
    get
    {
      return isRunPressed ? runMultiplier : walkMultiplier;
    }
  }

  private float Gravity
  {
    get
    {
      float timeToFall = isFalling() ? jumpTimeToFall : jumpTimeToApex;
      return (-2 * maxJumpHeight) / Mathf.Pow(timeToFall, 2);
    }
  }

  private float JumpVelocity
  {
    get
    {
      return (2 * maxJumpHeight) / jumpTimeToApex;
    }
  }

  private Quaternion InputTargetRotation
  {
    get
    {
      Vector3 lookTarget;

      lookTarget = transform.position - Camera.main.transform.position;

      lookTarget.y = zero; // So we don't look up or down

      return Quaternion.LookRotation(lookTarget.normalized, Vector3.up);
    }
  }

  private Quaternion OverrideTargetRotation
  {
    get
    {
      Vector3 lookTarget = (Vector3)overrideRotation - transform.position;
      lookTarget.y = 0f;

      // translate target to rotation
      return Quaternion.LookRotation(lookTarget.normalized, Vector3.up);
    }
  }

  private void ClearOverrideWhenDone(Quaternion targetRotation)
  {
    float distance = Quaternion.Angle(transform.rotation, targetRotation);
    bool isAtDestination = distance < .1;

    if (isAtDestination)
    {
      overrideRotation = null;
      overrideRotationSpeed = new float();
      return;
    }
  }


  private Vector3 InputToMotion
  {
    get
    {
      Vector3 cameraRelativeMotion = currentMovement;

      // apply horizontal speed multiplers
      cameraRelativeMotion.x *= this.SpeedMultiplier;
      cameraRelativeMotion.z *= this.SpeedMultiplier;

      // translate to camera relative direction
      cameraRelativeMotion = UnityService.LocalToWorldSpace(cameraRelativeMotion);
      // apply jump/gravity
      cameraRelativeMotion += currentVerticalMovement;

      return cameraRelativeMotion * UnityService.GetDeltaTime();
    }
  }

  private Vector3? OverrideToMotion
  {
    get
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
  }
  // End Utilities
}
