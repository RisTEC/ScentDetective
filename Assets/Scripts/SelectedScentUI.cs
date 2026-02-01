using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
public class SelectedScentUI : MonoBehaviour
{
    public TextMeshProUGUI scentNameText;
    private List<GameObject> models;
    public Button sniffButton;
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
        // Set name of character
        string displayText = "";
        if (ScentManager.Instance.characterSmells.ContainsKey(scent))
        {
            displayText += ScentManager.Instance.characterSmells[scent] + ": ";
        }
        scentNameText.text =  displayText + scent;

        // Display correct model
        foreach(GameObject model in models)
        {
            model.GetComponent<MeshRenderer>().enabled = 
                model.name == ScentManager.Instance.characterSmells[scent];
        }

        // Change color of button
        sniffButton.image.color = ScentManager.Instance.scentColors[scent];
        if(sniffButton.image.color.grayscale < 0.3f)
        {
            // Make text white if the color is too dark
            sniffButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.75f, 0.75f, 0.75f);
        }
        else
        {
            sniffButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
    }
}
