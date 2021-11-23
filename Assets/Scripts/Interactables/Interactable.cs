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

  void OnTriggerExit(Collider otherCollider)
  {
    if (otherCollider.tag == "Player") HideTooltip();
  }

#nullable enable
  protected virtual void MouthInteraction()
  {
    SafePlayOneShot(audioClips.Mouth);
  }

  protected virtual void MouthInteraction(Transform? mouthTransform)
  {
    SafePlayOneShot(audioClips.Mouth);
  }

  protected virtual void PawInteraction()
  {
    SafePlayOneShot(audioClips.Paw);
  }

  protected virtual void PawInteraction(Transform? playerTransform)
  {
    SafePlayOneShot(audioClips.Paw);
  }

  protected virtual void BodyInteraction()
  {
    SafePlayOneShot(audioClips.Body);
  }

  protected virtual void BodyInteraction(Transform? playerTransform)
  {
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
    return HandleInteraction(interactionType, null);
  }

  public PlayerAnimation HandleInteraction(InteractionType interactionType, Transform? playerTransform)
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
        BodyInteraction(playerTransform);
        return this.playerAnimations.Body;
      case InteractionType.UsePaw:
        PawInteraction(playerTransform);
        return this.playerAnimations.Paw;
      default:
        MouthInteraction(playerTransform);
        return this.playerAnimations.Mouth;
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
