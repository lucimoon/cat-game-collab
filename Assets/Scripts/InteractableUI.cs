using System.Collections.Generic;
using UnityEngine;

public class InteractableUI : MonoBehaviour
{
  GameObject player;
  PlayerController playerController;

  Canvas interactionTipInstance;

  [System.Serializable]
  public struct InteractionOption
  {
    public string interactionTipText;
    public InteractionType interactionType;
  }

  public List<InteractionOption> interactionOptionList = new List<InteractionOption>();
  public Dictionary<string, string> interactionBindings;

  void Awake()
  {
    player = GameObject.FindGameObjectWithTag("Player");
    playerController = player.GetComponent<PlayerController>();
  }

  void Update()
  {
    if (interactionBindings == null)
    {
      interactionBindings = playerController.interactionBindings;
      initInteractionTip();
    }
  }

  void initInteractionTip()
  {
    if (interactionBindings != null)
    {
      // Set position of interaction tool tip (attempt here to take into account size of object + camera position)
      Quaternion rotation = Camera.main.transform.rotation;
      Vector3 position = gameObject.transform.position;
      position.y = gameObject.transform.localScale.y + position.y;
      position.x = gameObject.transform.localScale.x + position.x;
      position.z = gameObject.transform.localScale.z + position.z;

      // Create new instance of tool tip based on prefab
      interactionTipInstance = Instantiate(Resources.Load("GenericInteractionTip", typeof(Canvas)), position, rotation) as Canvas;
      interactionTipInstance.gameObject.SetActive(false);

      Transform tipParent = interactionTipInstance.transform.Find("TipParent");
      for (int i = 0; i < interactionOptionList.Count; i++)
      {
        SetInteractionTipText(tipParent, interactionOptionList[i], i);
      }
      tipParent.gameObject.SetActive(false);
    }
  }

  // Set the instructional interaction tool tip text
  void SetInteractionTipText(Transform tipParent, InteractionOption interactionOption, int index)
  {
    GameObject duplicateTipParent = Instantiate(tipParent.gameObject);
    duplicateTipParent.transform.SetParent(interactionTipInstance.transform, false);
    duplicateTipParent.name = $"TipParent_0{index}";

    RectTransform tipParentTransform = duplicateTipParent.GetComponent<RectTransform>();
    Transform tipDisplayText = interactionTipInstance.transform.Find($"TipParent_0{index}/InteractionText");
    Transform tipButtonText = interactionTipInstance.transform.Find($"TipParent_0{index}/ButtonKeyBackground/ButtonKeyText");

    tipParentTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 50f * index, tipParentTransform.rect.height);

    tipButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = keyBindingText(interactionOption.interactionType);
    tipDisplayText.GetComponent<TMPro.TextMeshProUGUI>().text = interactionOption.interactionTipText;
  }

  string keyBindingText(InteractionType interactionType)
  {
    if (interactionType == InteractionType.UseMouth) return interactionBindings["UseMouth"];
    if (interactionType == InteractionType.UseBody) return interactionBindings["UseBody"];
    return interactionBindings["UsePaw"];
  }

  public void ShowInteractionTip(bool isVisible)
  {
    if (interactionTipInstance != null)
    {
      interactionTipInstance.gameObject.SetActive(isVisible);
    }
  }
}
