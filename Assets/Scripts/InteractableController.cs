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

  [System.Serializable]
  public struct IrritationReaction
  {
    public NonPlayerController nonPlayerController;
    public float irritationModifier;
  }
  public List<IrritationReaction> irritationReactionList = new List<IrritationReaction>();
  Canvas interactionTipInstance;

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
      handleUpdateIrritation(irritation.nonPlayerController, irritation.irritationModifier);
    }
    // Set the game object to inactive & destroy the interaction tool tip
    if (!allowMultipleInteractions)
    {
      gameObject.SetActive(false);
      Destroy(interactionTipInstance.gameObject);
    }
  }

  // Uses BroadcastMessage to send irritation to specific NPC
  public void handleUpdateIrritation(NonPlayerController nonPlayerController, float irritationModifier)
  {
    nonPlayerController.BroadcastMessage("UpdateIrritation", irritationModifier);
  }
}
