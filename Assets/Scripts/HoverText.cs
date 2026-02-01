using UnityEngine;
using TMPro;

public class HoverTextUI : MonoBehaviour
{
    public static HoverTextUI Instance;

    public TextMeshProUGUI text;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Show(string suspectName)
    {
        text.text = $"Is it the {suspectName}?";
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
