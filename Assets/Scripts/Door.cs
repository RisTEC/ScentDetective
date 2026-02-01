using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string accusationSceneName = "AccusationScene";
    public GameObject confirmPanel;

    void Start()
    {
        if (confirmPanel != null)
            confirmPanel.SetActive(false);
    }

    void OnMouseDown()
    {
        if (confirmPanel != null)
            confirmPanel.SetActive(true);
            UIBlocker.UIActive = true;
    }

    public void ConfirmAccusation()
    {
        SceneManager.LoadScene(accusationSceneName);
    }

    public void CancelAccusation()
    {
        if (confirmPanel != null)
            confirmPanel.SetActive(false);
            UIBlocker.UIActive = false;
    }
}