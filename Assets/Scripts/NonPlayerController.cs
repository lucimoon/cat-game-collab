using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NonPlayerController : MonoBehaviour

{
  [Header("Irritation Settings")]
  public float startingIrritation = 100f;
  public float minIrritation = 0f;
  public float maxIrritation = 200f;
  public float irritationScore;
  public IrritationBar irritationBar;

  void Awake()
  {
    irritationScore = startingIrritation;
  }
}
