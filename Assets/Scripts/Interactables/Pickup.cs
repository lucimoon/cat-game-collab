using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Interactable
{
  [SerializeField] private Transform mouthAttachmentPoint;
  private bool isHeld = false;

  protected override void MouthInteraction()
  {

    if (isHeld)
    {
      // Enable Gravity
      Rigidbody rigidbody = GetComponent<Rigidbody>();
      rigidbody.useGravity = true;
      rigidbody.isKinematic = false;

      // Parent to player
      transform.SetParent(null);
      this.playerAnimations.Mouth = PlayerAnimation.None;

      isHeld = false;
    }
    else
    {
      if (mouthAttachmentPoint != null)
      {
        this.playerAnimations.Mouth = PlayerAnimation.Eat;
        //Disable Gravity
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;

        // Parent to player
        transform.SetParent(mouthAttachmentPoint);
        //Attach to mouth
        transform.position = mouthAttachmentPoint.position;

        isHeld = true;
      }
    }
  }
}
