using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NonPlayerController : MonoBehaviour

{
  public float startingIrritation = 100f;
  public float minIrritation = 0f;
  public float maxIrritation = 200f;
  public float irritationScore;
  public IrritationBar irritationBar;

  // Declare reference variables
  Material npcMaterial;

  void Start()
  {
    irritationScore = startingIrritation;
    npcMaterial = GetComponent<Renderer>().material;
  }

  // UpdateIrritation triggered by InteractableController
  private void UpdateIrritation(float irritationModifier)
  {
    float newIrritationScore = irritationScore + irritationModifier;
    // colors for testing
    Color red = new Color(0.6981f, 0.5160248f, 0.4625756f, 1f);
    Color green = new Color(0.494838f, 0.754717f, 0.6536991f, 1f);
    Color gray = new Color(0.6981f, 0.6981f, 0.6981f, 1f);

    // flash colors for testing
    if (newIrritationScore > irritationScore)
    {
      StartCoroutine(ResetMaterial(red, gray));
    }
    else if (newIrritationScore < irritationScore)
    {
      StartCoroutine(ResetMaterial(green, gray));
    }

    // Update score if within threshold
    if (newIrritationScore >= minIrritation && newIrritationScore <= maxIrritation)
    {
      irritationScore += irritationModifier;
      irritationBar.UpdateIrritationBar();
    }
  }

  IEnumerator ResetMaterial(Color startColor, Color resetColor)
  {
    npcMaterial.color = startColor;
    yield return new WaitForSeconds(1);
    npcMaterial.color = resetColor;
  }
}
