using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class SelectedScentUI : MonoBehaviour
{
    public TextMeshProUGUI scentNameText;
    private List<GameObject> models;
    public void Awake()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Character"))
            {
                models.Add(child.gameObject);
            }
        }
    }
    public void SelectScent(String scent)
    {
        Debug.Log("Scent selected: " + scent);
        string displayText = "";
        if (ScentManager.Instance.characterSmells.ContainsKey(scent))
        {
            displayText += ScentManager.Instance.characterSmells[scent] + ": ";
        }
        scentNameText.text =  displayText + scent;
        foreach(GameObject model in models)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = 
                gameObject.name == ScentManager.Instance.characterSmells[scent];
        }
    }
}
