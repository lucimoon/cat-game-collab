using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
  public bool allowMultipleInteractions = false;
  IrritationBar irritationBar;
  InteractableUI interactableUI;

  [System.Serializable]
  public struct IrritationReaction
  {
    public NonPlayerController nonPlayerController;
    public float irritationModifier;
  }
  public List<IrritationReaction> irritationReactionList = new List<IrritationReaction>();
  NonPlayerController reactingNPC;

  [SerializeField] private PlayerAnimation mouthPlayerAnimation;
  [SerializeField] private PlayerAnimation pawPlayerAnimation;
  [SerializeField] private PlayerAnimation bodyPlayerAnimation;

  void Start()
  {
    interactableUI = GetComponent<InteractableUI>();
  }

  public PlayerAnimation HandleInteraction(InteractionType interactionType)
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
    switch (interactionType)
    {
      case InteractionType.UseBody:
        BodyInteraction();
        return bodyPlayerAnimation;
      case InteractionType.UsePaw:
        PawInteraction();
        return pawPlayerAnimation;
      default:
        MouthInteraction();
        return mouthPlayerAnimation;
    }
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

  protected virtual void MouthInteraction()
  {
    Debug.Log("MouthInteraction");
  }

  protected virtual void PawInteraction()
  {
    Debug.Log("PawInteraction");
  }

  protected virtual void BodyInteraction()
  {
    Debug.Log("BodyInteraction");
  }

  public void ShowTooltip()
  {
    interactableUI.ShowInteractionTip(true);
  }

  public void HideTooltip()
  {
    interactableUI.ShowInteractionTip(true);
  }
}
