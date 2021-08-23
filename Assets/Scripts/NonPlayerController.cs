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
  CharacterController characterController;
  Color color;

  void Start()
  {
    irritationScore = startingIrritation;
  }

  // UpdateIrritation triggered by InteractableController
  private void UpdateIrritation(float irritationModifier)
  {
    float newIrritationScore = irritationScore + irritationModifier;

    if (newIrritationScore >= minIrritation && newIrritationScore <= maxIrritation)
    {
      irritationScore += irritationModifier;
      irritationBar.UpdateIrritationBar();
    }

  }
}
