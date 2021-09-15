using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
  public bool allowMultipleInteractions = false;
  IrritationBar irritationBar;

  [System.Serializable]
  public struct IrritationReaction
  {
    public NonPlayerController nonPlayerController;
    public float irritationModifier;
  }
  public List<IrritationReaction> irritationReactionList = new List<IrritationReaction>();
  NonPlayerController reactingNPC;

  public void HandleInteraction()
  {
    // Loop through each attached NPC irritation reaction
    foreach (IrritationReaction irritation in irritationReactionList)
    {
      setIrritationScore(irritation);
    }
    // Set the game object to inactive & destroy the interaction tool tip
    if (!allowMultipleInteractions)
    {
      // Todo: Make sure that interaction tip is also destroyed/set inactive
      gameObject.SetActive(false);
    }

    // Todo: Setup max interactions
  }

  // UpdateIrritation triggered by InteractableController
  void setIrritationScore(IrritationReaction irritation)
  {
    reactingNPC = irritation.nonPlayerController;
    irritationBar = reactingNPC.irritationBar;

    Transform npc = reactingNPC.transform;
    FieldOfView fieldOfView = npc.GetComponent(typeof(FieldOfView)) as FieldOfView;
    float newIrritationScore = reactingNPC.irritationScore + irritation.irritationModifier;

    if (fieldOfView.visibleInteractables.Contains(this.transform))
    {
      Debug.Log($"{npc.name} saw you messing with {this}!");

      // Update score if within threshold
      if (newIrritationScore >= reactingNPC.minIrritation && newIrritationScore <= reactingNPC.maxIrritation)
      {
        reactingNPC.irritationScore += irritation.irritationModifier;
        irritationBar.UpdateIrritationBar();
      }
    }
    else
    {
      Debug.Log($"{npc.name} didn't see you messing with {this}...");
    }
  }
}
