using System;
using UnityEngine;
public class GridTile : MonoBehaviour
{
    public Vector2Int gridPos;
    public bool walkable = true;
    public float level = 0; 
    public bool isStairs = false; 
    public Renderer gridRenderer; 
    private Color originalColor;
    public Color hoverColor = Color.yellow;
    
    void Awake()
    {
        gridPos = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.z)
        );
        level = Mathf.Round(transform.position.y / 3f * 4f) / 4f;
        gridRenderer = GetComponentInChildren<Renderer>();
        if (gridRenderer != null)
        {
            originalColor = gridRenderer.material.color;
        }
    }
    public void UpdateGridInfo()
    {
        // Calculate postion and level
        gridPos = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.z)
        );
        level = Mathf.Round(transform.position.y / 3f * 4f) / 4f;

        // Raycast to check if an obstacle is above
        walkable = !Physics.Raycast(transform.position, Vector3.up, Mathf.Infinity, LayerMask.GetMask("Obstacles"));
        Debug.DrawRay(transform.position, Vector3.up, walkable ? Color.green : Color.red, 10.0f);
    } 

    public void Highlight()
    {
        if (gridRenderer != null)
        {
            gridRenderer.material.color = hoverColor;
        }
    }
    
    public void Unhighlight()
    {
        if (gridRenderer != null)
        {
            gridRenderer.material.color = originalColor;
        }
    }
}