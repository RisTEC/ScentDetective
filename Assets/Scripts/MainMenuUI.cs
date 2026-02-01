using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public string firstGameScene = "MainSceneCrime 1";

    public void StartGame()
    {
        // Reset game state if replaying
        GameManager.Instance.gameEnded = false;

        SceneManager.LoadScene(firstGameScene);
    }
}
