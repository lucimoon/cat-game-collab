using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
  // This helps visualize the NPC's viewable cone by showing the area they can see in the editor view
  void OnSceneGUI()
  {
    FieldOfView fov = (FieldOfView)target;
    Handles.color = Color.white;
    Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);
    Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
    Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);

    Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
    Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);

    // Indicate when an interactable is visible
    Handles.color = Color.red;
    foreach (Transform visibleInteractable in fov.visibleInteractables)
    {
      Handles.DrawLine(fov.transform.position, visibleInteractable.position);
    }
  }
}
