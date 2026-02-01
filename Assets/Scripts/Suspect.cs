using UnityEngine;

public class Suspect : MonoBehaviour
{
    public string suspectName;

    void OnMouseEnter()
    {
        if (AccusationUI.IsConfirming) return;
        SpotlightController.Instance.FocusOn(transform);
        HoverTextUI.Instance.Show(suspectName);
    }

    void OnMouseExit()
    {
        if (AccusationUI.IsConfirming) return;
        SpotlightController.Instance.ClearFocus();
        HoverTextUI.Instance.Hide();
    }

    void OnMouseDown()
    {
        if (AccusationUI.IsConfirming) return;
        AccusationUI.Instance.Show(suspectName);
    }
}
