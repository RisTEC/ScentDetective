using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public System.Action OnDialogueFinished;
    Queue<string> lines = new Queue<string>();
    bool active = false;

    void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (!active) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextLine();
        }
    }

    public void StartDialogue(List<string> dialogueLines)
    {
        lines.Clear();
        foreach (var line in dialogueLines)
            lines.Enqueue(line);

        dialoguePanel.SetActive(true);
        active = true;
        UIBlocker.UIActive = true;

        ShowNextLine();
    }

    void ShowNextLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = lines.Dequeue();
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        active = false;
        UIBlocker.UIActive = false;

        OnDialogueFinished?.Invoke();
        OnDialogueFinished = null;
    }
}
