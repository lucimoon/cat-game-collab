using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using NSubstitute;

public class InputTests : InputTestFixture
{
  // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
  // `yield return null;` to skip a frame.
  [UnityTest]
  public IEnumerator PlayerPrefabUsesInputComponent()
  {
    // Use the Assert class to test conditions.
    // Use yield to skip a frame.
    PlayerController player = CreatePlayer();
    Assert.That(player.GetComponent<PlayerInput>(), Is.Not.Null);

    yield return null;
  }

  [UnityTest]
  public IEnumerator PlayerCanMoveForward()
  {
    // Use the Assert class to test conditions.
    // Use yield to skip a frame.
    PlayerController playerController = CreatePlayer();
    playerController.UnityService = Substitute.For<IUnityService>();
    Keyboard keyboard = InputSystem.AddDevice<Keyboard>();

    playerController.UnityService.GetDeltaTime().Returns<float>(1f);


    Debug.Log(playerController.transform.position);

    Press(keyboard.wKey);

    yield return null;
    Debug.Log(playerController.transform.position);
    Assert.That(playerController.transform.position, Is.Not.Null);

    yield return null;
  }

  // Utilities
  private PlayerController CreatePlayer()
  {
    PlayerController player = AssetDatabase.LoadAssetAtPath<PlayerController>("Assets/Prefabs/Characters/Player.prefab");
    return player;
  }
}
