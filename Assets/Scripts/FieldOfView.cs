using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// See https://www.youtube.com/watch?v=rQG9aUWarwE 

public class FieldOfView : MonoBehaviour
{
  public float viewRadius;
  [Range(0, 360)]
  public float viewAngle;
  public LayerMask whatIsScene, whatIsPlayer, whatIsInteractable;
  public List<Transform> visibleInteractables = new List<Transform>();

  void Start()
  {
    StartCoroutine("FindInteractablesWithDelay", .3f);
  }

  IEnumerator FindInteractablesWithDelay(float delay)
  {
    while (true)
    {
      yield return new WaitForSeconds(delay);
      FindVisibleInteractables();
    }
  }

  void FindVisibleInteractables()
  {
    visibleInteractables.Clear();
    Collider[] interactablesInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, whatIsInteractable);

    for (int i = 0; i < interactablesInViewRadius.Length; i++)
    {
      Transform interactable = interactablesInViewRadius[i].transform;
      Vector3 dirToInteractable = (interactable.position - transform.position).normalized;
      if (Vector3.Angle(transform.forward, dirToInteractable) < viewAngle / 2)
      {
        float distanceToInteractable = Vector3.Distance(transform.position, interactable.position);
        if (!Physics.Raycast(transform.position, dirToInteractable, distanceToInteractable, whatIsScene))
        {
          visibleInteractables.Add(interactable);
        }
      }
    }
  }
  public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
  {
    if (!angleIsGlobal)
    {
      angleInDegrees += transform.eulerAngles.y;
    }
    return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
  }

}
