using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableController : MonoBehaviour
{
  [System.Serializable]
  public struct IrritationReaction
  {
    public NonPlayerController nonPlayerController;
    public float irritationModifier;
  }
  public List<IrritationReaction> irritationReactionList = new List<IrritationReaction>();

  // Sample trigger
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      foreach (IrritationReaction irritation in irritationReactionList)
      {
        handleUpdateIrritation(irritation.nonPlayerController, irritation.irritationModifier);
      }
    }
  }

  // Uses BroadcastMessage to send irritation to specific NPC
  private void handleUpdateIrritation(NonPlayerController nonPlayerController, float irritationModifier)
  {
    nonPlayerController.BroadcastMessage("UpdateIrritation", irritationModifier);
  }
}
