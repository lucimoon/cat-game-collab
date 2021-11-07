using UnityEngine;

#nullable enable
public class Peeable : Interactable
{
  public float speed = 1f;
  public float rotationSpeed = 1f;
  public float timeElapsed = 0f;
  public Transform? playerTarget;
  private Transform? playerTransform;
  private PlayerController? controller;

  protected override void BodyInteraction(Transform? playerTransform)
  {
    if (playerTransform == null) return;
    if (playerTarget == null) playerTarget = transform;

    controller = playerTransform.GetComponent<PlayerController>();

    if (controller != null)
    {
      controller.OverrideMovement(playerTarget.position, speed);
      controller.OverrideRotation(rotationSpeed);
    }
  }
}
