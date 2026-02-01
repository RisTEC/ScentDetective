using UnityEngine;

public class Suspect : MonoBehaviour
{
    public string suspectName;

    void OnMouseEnter()
    {
        SpotlightController.Instance.FocusOn(transform);
    }

    void OnMouseExit()
    {
        SpotlightController.Instance.ClearFocus();
    }

    void OnMouseDown()
    {
        GameManager.Instance.Accuse(suspectName);
    }
}
