using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doohickey : Interactable
{
  public float interactionForce = 10f;

  protected override void MouthInteraction()
  {
    base.MouthInteraction();
    MoveUpwards();
  }

  protected override void BodyInteraction()
  {
    base.BodyInteraction();
    MoveUpwards();
  }

  protected override void PawInteraction()
  {
    base.PawInteraction();
    MoveUpwards();
  }

  private void MoveUpwards()
  {
    Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
    rigidbody.AddForce(Vector3.up * interactionForce, ForceMode.Impulse);
  }
}
