using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class Pickup : Interactable
{
  [SerializeField] private Transform? mouthAttachmentPoint;
  private PlayerAnimation DETATCH_ANIMATION = PlayerAnimation.None;
  private PlayerAnimation ATTACH_ANIMATION = PlayerAnimation.None;
  private bool isHeld = false;
  private new Rigidbody? rigidbody;

  protected override void Start()
  {
    base.Start();
    rigidbody = GetComponent<Rigidbody>();
  }

  protected override void MouthInteraction(Transform? attachmentPoint)
  {
    if (isHeld)
    {
      Detatch();
    }
    else
    {
      Attach(attachmentPoint);
    }
  }

  private void Detatch()
  {
    isHeld = false;

    if (rigidbody != null)
    {
      rigidbody.useGravity = true;
      rigidbody.isKinematic = false;
    }

    transform.SetParent(null);

    this.playerAnimations.Mouth = DETATCH_ANIMATION;
  }

  private void Attach(Transform? attachmentPoint)
  {
    if (rigidbody == null) throw new System.Exception("No Rigidbody attached to Pickup");
    if (attachmentPoint == null) throw new System.Exception("No attachment point given to Pickup");

    isHeld = true;

    rigidbody.useGravity = false;
    rigidbody.isKinematic = true;

    this.playerAnimations.Mouth = ATTACH_ANIMATION;

    transform.SetParent(attachmentPoint);
    transform.position = attachmentPoint.position;
  }
}
