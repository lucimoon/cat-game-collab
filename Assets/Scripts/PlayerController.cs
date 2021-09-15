using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  // Interface variables
  [Header("Speed Multipliers")]
  [SerializeField] private float walkMultiplier = 3.0f;
  [SerializeField] private float runMultiplier = 8.0f;

  [Header("Jump Parameters")]
  [SerializeField] private float maxJumpHeight = 6.0f;
  [SerializeField] private float jumpTimeToApex = 0.45f;
  [SerializeField] private float jumpTimeToFall = 0.2f;
  [Tooltip("Distance before fall animation is triggered")]
  [SerializeField] private float fallHeight = 0.075f;

  // Declare reference variables
  PlayerInput playerInput;
  CharacterController characterController;
  Animator animator;
  Interactable interactable;
  InteractableUI interactableUI;

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

  // Interact
  public bool isInteractPressed;
  InputAction interactAction;
  GameObject interactedObject;

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
    move();
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
    interactedObject = other.gameObject;
    if (interactedObject.CompareTag("Interactable"))
    {
      // Set our script reference to our newly collided interactable gameobject
      interactable = interactedObject.GetComponent(typeof(Interactable)) as Interactable;
      interactableUI = interactedObject.GetComponent(typeof(InteractableUI)) as InteractableUI;

      // Show the interaction tool tip
      interactableUI.ShowInteractionTip(true);
    }
  }

  void OnTriggerExit()
  {
    // Hide the interaction tool tip
    interactableUI.ShowInteractionTip(false);
    // When we leave, set the current interactable to null.
    interactedObject = null;
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

  private void UseObjectInteraction(string interactionName)
  {
    InteractionType interactionType = InteractionType.UseMouth;

    switch (interactionName)
    {
      case "UsePaw":
        interactionType = InteractionType.UsePaw;
        break;
      case "UseBody":
        interactionType = InteractionType.UseBody;
        break;
      default:
        break;
    }

    interactable.HandleInteraction(interactionType);
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

  private void UsePaw() { Debug.Log("UsePaw"); }
  private void Meow() { Debug.Log("Meow"); }
  private void TakeRest() { Debug.Log("TakeRest"); }
}
