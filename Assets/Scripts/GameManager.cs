using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Suspects")]
    public List<string> suspects;
    public string culprit; // set in inspector

    [Header("State")]
    public bool gameEnded = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Accuse(string suspectName)
    {
        if (gameEnded)
            return;

        gameEnded = true;

        if (suspectName == culprit)
        {
            DialogueManager.Instance.StartDialogue(
                new List<string>
                {
                "You piece together the clues.",
                suspectName + " is the murderer.",
                "Justice is served."
                }
            );
        }
        else
        {
            DialogueManager.Instance.StartDialogue(
                new List<string>
                {
                "Something feels wrong.",
                suspectName + " wasn't the killer.",
                "The real culprit got away."
                }
            );
        }

        DialogueManager.Instance.OnDialogueFinished = ReturnToMainMenu;
    }

    void ReturnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

}
