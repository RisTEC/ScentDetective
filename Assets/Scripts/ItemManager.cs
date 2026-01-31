using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private List<Item> items;
    public List<Item> Items
    {
        get
        {
            return items;
        }
    }

    public static ItemManager Instance;

    void Start()
    {
        Instance = this;    
    }

    /// <summary>
    /// Call this method to update any new items added to scene. 
    /// Automatically assigns a gridPos and layer, which can manually be changed later.
    /// </summary>
    [ContextMenu("Update Item Location")]
    public void AssignTiles()
    {
        // Get item script from children
        items = new List<Item>(this.GetComponentsInChildren<Item>());
        
        int unassignedItems = 0;
        foreach(Item item in items)
        {
            if(item.ManualLocation){continue;}
            if (!item.AssignLocation())
            {
                unassignedItems++;
            }
            else
            {
                // Makes it so the values don't revert to default when game is started
                EditorUtility.SetDirty(item);
            }
        }
        Debug.Log("Items that need assigning: " + unassignedItems);
    }
}
