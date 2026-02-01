using UnityEngine;
using TMPro;

public class AccusationUI : MonoBehaviour
{
    public static bool IsConfirming;
    public static AccusationUI Instance;
    public GameObject panel;

    public TextMeshProUGUI confirmText;

    private string suspectName;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
        IsConfirming = false;
    }

    public void Show(string name)
    {
        HoverTextUI.Instance.Hide();
        SpotlightController.Instance.ClearFocus();
        IsConfirming = true;
        suspectName = name;
        confirmText.text =
            $"You accuse {name}.\n\n" +
            "Is this who you really think did it?";
        panel.SetActive(true);
    }

    public void Confirm()
    {
        panel.SetActive(false);
        IsConfirming = true;
        GameManager.Instance.Accuse(suspectName);
    }

    public void Cancel()
    {
        IsConfirming = false;
        suspectName = null;
        panel.SetActive(false);
    }
}
