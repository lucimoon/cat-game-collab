using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnityService
{
  float GetDeltaTime();
  Vector3 LocalToWorldSpace(Vector3 localVector);
}
