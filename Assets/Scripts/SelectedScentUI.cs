using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class SelectedScentUI : MonoBehaviour
{
    public TextMeshProUGUI scentNameText;
    private List<GameObject> models;
    public void Awake()
    {
        models = new List<GameObject>();
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
            Debug.Log("Testing: " + model.name);
            if(model.name == ScentManager.Instance.characterSmells[scent])
            {
                Debug.Log("Showing: " + model.name);
            }
            model.GetComponent<MeshRenderer>().enabled = 
                model.name == ScentManager.Instance.characterSmells[scent];
        }
    }
}
