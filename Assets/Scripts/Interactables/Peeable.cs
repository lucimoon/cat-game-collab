using UnityEngine;

#nullable enable
public class Peeable : Interactable
{
  [Space]
  [Tooltip("Where the player moves to at start of interaction")]
  [SerializeField] private Transform? interactionTarget;

  [Header("Input Override Speeds")]
  [Tooltip("How fast player moves to interactionTarget")]
  [Range(0f, 1f)]
  [SerializeField] private float speed = 0.1f;
  [Tooltip("How fast player turns around mid-animation")]
  [Range(1f, 10f)]
  [SerializeField] private float rotationSpeed = 6.5f;

  protected override void Start()
  {
    base.Start();
    this.playerAnimations.Body = PlayerAnimation.Pee;
  }

  protected override void BodyInteraction(Transform? playerTransform)
  {
    PlayerController? controller;

    if (playerTransform == null) return;
    if (interactionTarget == null) interactionTarget = transform;

    controller = playerTransform.GetComponent<PlayerController>();

    if (controller != null)
    {
      controller.OverrideMovement(interactionTarget.position, speed);
      controller.OverrideRotation(rotationSpeed);
    }
  }
}
