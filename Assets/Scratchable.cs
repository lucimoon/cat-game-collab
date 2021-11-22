using UnityEngine;

#nullable enable
[RequireComponent(typeof(BoxCollider))]
public class Scratchable : Interactable
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
    SetTag();
    SetAnimation();
  }

  protected override void PawInteraction(Transform? playerTransform)
  {
    PlayerController? controller;

    if (playerTransform == null) return;
    if (interactionTarget == null) interactionTarget = transform;
    Vector3 finalTarget = interactionTarget.position;
    finalTarget.y = playerTransform.position.y;

    controller = playerTransform.GetComponent<PlayerController>();

    if (controller != null)
    {
      controller.OverrideMovement(finalTarget, speed);
      controller.OverrideRotation(transform.position, rotationSpeed);
    }
  }

  private void SetTag()
  {
    gameObject.tag = "Interactable";
  }

  private void SetAnimation()
  {
    playerAnimations.Body = PlayerAnimation.Pee;
  }
}
