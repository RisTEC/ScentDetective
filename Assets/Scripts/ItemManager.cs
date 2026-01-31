using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private List<Item> items;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        
        // Raycast down from each item to find a matching gridTile
        int unassignedItems = 0;
        foreach(Item item in items)
        {
            if(item.ManualLocation){continue;}

            RaycastHit hitInfo;
            Debug.DrawRay(item.transform.position, Vector3.down, Color.blue, 10.0f);
            if(Physics.Raycast(
                item.transform.position, Vector3.down, out hitInfo , Mathf.Infinity, LayerMask.GetMask("Floor")))
            {
                // Copy gridTile info over
                GridTile hitTile= hitInfo.collider.GetComponent<GridTile>();
                item.gridPos = hitTile.gridPos;
                item.level = hitTile.level;
            }
            else
            {
                // Default info because there is no tile
                item.gridPos = Vector2Int.zero;
                item.level = 0;
                unassignedItems++;
            }
        }
        Debug.Log("Items that need assigning: " + unassignedItems);
    }
}
