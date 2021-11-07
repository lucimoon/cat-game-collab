using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(InteractableUI))]
public class Interactable : MonoBehaviour
{
  public bool allowInteractions = true;
  public bool isDestroyed = false;
  [SerializeField] private List<IrritationReaction> irritationReactionList = new List<IrritationReaction>();
  [SerializeField] private InteractableAudio audioClips;
  [SerializeField] protected InteractablePlayerAnimation playerAnimations;

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

  protected virtual void LateUpdate()
  {
    if (isDestroyed)
    {
      interactableUI.DestroyInteractionTip();
    }
  }

  protected virtual void MouthInteraction()
  {
    Debug.Log("MouthInteraction");
    SafePlayOneShot(audioClips.Mouth);
  }

#nullable enable
  protected virtual void MouthInteraction(Transform? attachmentPoint)
  {
    SafePlayOneShot(audioClips.Mouth);
  }
#nullable disable

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

#nullable enable
  protected virtual void BodyInteraction(Transform? playerTransform)
  {
    SafePlayOneShot(audioClips.Body);
  }
#nullable disable

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
    return HandleInteraction(interactionType, null);
  }

#nullable enable
  public PlayerAnimation HandleInteraction(InteractionType interactionType, Transform? attachmentPoint)
  {
    // Loop through each attached NPC irritation reaction
    foreach (IrritationReaction irritation in irritationReactionList)
    {
      SetIrritationScore(irritation);
    }

    // Todo: Setup max interactions
    switch (interactionType)
    {
      case InteractionType.UseBody:
        BodyInteraction(attachmentPoint);
        return playerAnimations.Body;
      case InteractionType.UsePaw:
        PawInteraction();
        return playerAnimations.Paw;
      default:
        MouthInteraction(attachmentPoint);
        return playerAnimations.Mouth;
    }
  }
#nullable disable

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
