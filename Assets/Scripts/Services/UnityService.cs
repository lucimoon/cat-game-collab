using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityService : IUnityService
{
  public float GetDeltaTime()
  {
    return Time.deltaTime;
  }

  public Vector3 LocalToWorldSpace(Vector3 localVector)
  {
    return Camera.main.transform.TransformVector(localVector);
  }
}
