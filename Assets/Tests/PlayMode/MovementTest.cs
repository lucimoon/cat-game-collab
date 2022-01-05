using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using NSubstitute;

public class MovementTest : InputTestFixture
{
  PlayerController playerController;
  Camera camera;
  Keyboard keyboard;

  // Setup
  private void BeforeEach()
  {
    playerController = CreatePlayer();
    camera = CreateCamera(playerController.transform.position);
    keyboard = InputSystem.AddDevice<Keyboard>();
  }

  // Tests
  [UnityTest]
  public IEnumerator PlayerCanMoveAwayFromCamera()
  {
    // Arrange
    BeforeEach();
    float initialDistance = Vector3.Distance(playerController.transform.position, camera.transform.position);

    // Act
    Press(keyboard.wKey);
    yield return null;

    // Assert
    Assert.That(
      Vector3.Distance(playerController.transform.position, camera.transform.position),
      Is.GreaterThan(initialDistance)
      );
  }

  [UnityTest]
  public IEnumerator PlayerCanMoveTowardCamera()
  {
    BeforeEach();
    float initialDistance = Vector3.Distance(playerController.transform.position, camera.transform.position);

    Press(keyboard.sKey);
    yield return null;

    Assert.That(
      Vector3.Distance(playerController.transform.position, camera.transform.position),
      Is.LessThan(initialDistance)
      );
  }

  [UnityTest]
  public IEnumerator PlayerCanMoveLeftOfCamera()
  {
    BeforeEach();
    float initialPositionX = camera.WorldToScreenPoint(playerController.transform.position).x;

    Press(keyboard.aKey);
    yield return null;

    Assert.That(
      camera.WorldToScreenPoint(playerController.transform.position).x,
      Is.LessThan(initialPositionX)
      );
  }


  [UnityTest]
  public IEnumerator PlayerCanMoveRightOfCamera()
  {
    BeforeEach();
    float initialPositionX = camera.WorldToScreenPoint(playerController.transform.position).x;

    Press(keyboard.dKey);
    yield return null;

    Assert.That(
      camera.WorldToScreenPoint(playerController.transform.position).x,
      Is.GreaterThan(initialPositionX)
      );
  }

  // Utilities
  private PlayerController CreatePlayer()
  {
    PlayerController prefab = AssetDatabase.LoadAssetAtPath<PlayerController>("Assets/Prefabs/Characters/Player.prefab");
    return Object.Instantiate<PlayerController>(prefab, Vector3.zero, Quaternion.identity);
  }

  private Camera CreateCamera(Vector3 lookTarget)
  {
    GameObject camera = new GameObject("Main Camera", typeof(Camera));
    camera.tag = "MainCamera";
    camera.transform.position = new Vector3(-10f, 10f, 5f);
    camera.transform.LookAt(lookTarget);


    return camera.GetComponent<Camera>();
  }
}
