using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
  public string nextScene;
  public void StartGame()
  {
    Debug.Log("Start Game");
    SceneManager.LoadScene(nextScene);
  }

  public void QuitGame()
  {
    Debug.Log("Quit");
    Application.Quit();
  }
}