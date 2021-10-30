using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class Pickup : Interactable
{
  private bool isHeld = false;

  protected override void MouthInteraction(Transform? attachmentPoint)
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
      if (attachmentPoint != null)
      {
        this.playerAnimations.Mouth = PlayerAnimation.Eat;
        //Disable Gravity
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;

        // Parent to player
        transform.SetParent(attachmentPoint);
        //Attach to mouth
        transform.position = attachmentPoint.position;

        isHeld = true;
      }
    }
  }
}
