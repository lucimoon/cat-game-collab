using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Interactable : MonoBehaviour
{
  [SerializeField] private bool allowMultipleInteractions = false;
  public bool allowInteractions = true;
  [SerializeField] private List<IrritationReaction> irritationReactionList = new List<IrritationReaction>();
  [SerializeField] private InteractableAudio audioClips;
  [SerializeField] private InteractablePlayerAnimation playerAnimations;

  [System.Serializable]
  public struct IrritationReaction
  {
    public NonPlayerController nonPlayerController;
    public float irritationModifier;
  }

  private IrritationBar irritationBar;
  private InteractableUI interactableUI;
  private NonPlayerController reactingNPC;
  private AudioSource audioSource;

  protected virtual void Start()
  {
    interactableUI = GetComponent<InteractableUI>();
    audioSource = GetComponent<AudioSource>();
  }

  protected virtual void MouthInteraction()
  {
    Debug.Log("MouthInteraction");
    SafePlayOneShot(audioClips.Mouth);
  }

  protected virtual void PawInteraction()
  {
    Debug.Log("PawInteraction");
    SafePlayOneShot(audioClips.Paw);
  }

  protected virtual void BodyInteraction()
  {
    Debug.Log("BodyInteraction");
    SafePlayOneShot(audioClips.Body);
  }

  public void ShowTooltip()
  {
    interactableUI.ShowInteractionTip(true);
  }

  public void HideTooltip()
  {
    interactableUI.ShowInteractionTip(false);
  }

  public PlayerAnimation HandleInteraction(InteractionType interactionType)
  {
    // Loop through each attached NPC irritation reaction
    foreach (IrritationReaction irritation in irritationReactionList)
    {
      SetIrritationScore(irritation);
    }
    // Set the game object to inactive & destroy the interaction tool tip
    if (!allowMultipleInteractions)
    {
      // Todo: Make sure that interaction tip is also destroyed/set inactive
      // gameObject.SetActive(false);
    }

    // Todo: Setup max interactions
    switch (interactionType)
    {
      case InteractionType.UseBody:
        BodyInteraction();
        return playerAnimations.Body;
      case InteractionType.UsePaw:
        PawInteraction();
        return playerAnimations.Paw;
      default:
        MouthInteraction();
        return playerAnimations.Mouth;
    }
  }

  // UpdateIrritation triggered by InteractableController
  private void SetIrritationScore(IrritationReaction irritation)
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

  private void SafePlayOneShot(AudioClip clip)
  {
    if (clip != null) audioSource.PlayOneShot(clip);
  }
}
