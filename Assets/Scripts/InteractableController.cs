using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableController : MonoBehaviour
{
  // Prefab reference
  public Canvas interactionTip;
  public string interactionTipText;
  public bool allowMultipleInteractions = false;
  IrritationBar irritationBar;

  [System.Serializable]
  public struct IrritationReaction
  {
    public NonPlayerController nonPlayerController;
    public float irritationModifier;
  }
  public List<IrritationReaction> irritationReactionList = new List<IrritationReaction>();
  Canvas interactionTipInstance;

  NonPlayerController reactingNPC;

  void Awake()
  {
    initInteractionTip();
  }

  void initInteractionTip()
  {
    if (interactionTip != null)
    {
      // Set position of interaction tool tip (attempt here to take into account size of object + camera position)
      Quaternion rotation = Camera.main.transform.rotation;
      Vector3 position = gameObject.transform.position;
      position.y = gameObject.transform.localScale.y + position.y;
      position.x = gameObject.transform.localScale.x + position.x;
      position.z = gameObject.transform.localScale.z + position.z;

      // Create new instance of tool tip based on prefab
      interactionTipInstance = Instantiate(interactionTip, position, rotation);
      interactionTipInstance.gameObject.SetActive(false);

      SetInteractionTipText();
    }
  }

  // Set the instructional interaction tool tip text
  void SetInteractionTipText()
  {
    TextMeshProUGUI tipDisplayText = interactionTipInstance.GetComponentInChildren<TMPro.TextMeshProUGUI>();
    tipDisplayText.text = interactionTipText;
  }

  public void ShowInteractionTip(bool isVisible)
  {
    if (interactionTipInstance != null)
    {
      interactionTipInstance.gameObject.SetActive(isVisible);
    }
  }

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
      gameObject.SetActive(false);
      Destroy(interactionTipInstance.gameObject);
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
}
