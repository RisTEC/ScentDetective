using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string accusationSceneName = "AccusationScene";

    void OnMouseDown()
    {
        SceneManager.LoadScene(accusationSceneName);
    }
}
