using UnityEngine;
using UnityEngine.SceneManagement;

public class CrimeSceneUI : MonoBehaviour
{
    public string firstGameScene = "MainSceneCrime 1";

    public void StartGame()
    {
        // Reset game state if replaying
        SceneManager.LoadScene(firstGameScene);
    }
}
