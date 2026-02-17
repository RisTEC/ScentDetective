using System;
using UnityEngine;
public class GridTile : MonoBehaviour
{
    public Vector2Int gridPos;
    public bool walkable = true;
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
        gridRenderer = GetComponentInChildren<Renderer>();
        if (gridRenderer != null)
        {
            originalColor = gridRenderer.material.color;
        }
    }
    public void UpdateGridInfo()
{
    // Calculate position
    gridPos = new Vector2Int(
        Mathf.RoundToInt(transform.position.x),
        Mathf.RoundToInt(transform.position.z)
    );
    
    // Raycast upward with a limited distance to check for obstacles
    Vector3 rayStart = transform.position + Vector3.down * 0.1f;
    walkable = !Physics.Raycast(rayStart, Vector3.up, 2f, LayerMask.GetMask("Obstacles"));
    
    Debug.DrawRay(rayStart, Vector3.up * 2f, walkable ? Color.green : Color.red, 10.0f);
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