using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
public class GridManager : MonoBehaviour
{
    public List<GridTile> tiles = new List<GridTile>();
    public static GridManager Instance;
    public void Start()
    {
        Instance = this;
    }

    /// <summary>
    /// Call to calculate grid info and walkability of all tiles
    /// </summary>
    [ContextMenu("Set initial tile data")]
    public void UpdateTileInfo()
    {
        tiles = new List<GridTile>(this.GetComponentsInChildren<GridTile>());

        // Get all stairs => get their GridTile script
        List<GridTile> stairs = new List<GridTile>(GameObject.FindGameObjectsWithTag("Stair").
            Select(stair => stair.GetComponent<GridTile>()));

        tiles.AddRange(stairs);
        
        foreach(GridTile tile in tiles)
        {
            tile.UpdateGridInfo();

            // Makes it so the values don't revert to default when game is started
            EditorUtility.SetDirty(tile);
        }
    }
    
    public bool IsWalkable(Vector2Int target, float currentLevel, Vector2Int from)
    {
        foreach (var tile in tiles)
        {
            if (tile.gridPos != target || !tile.walkable)
                continue;
            
            float levelDiff = Mathf.Abs(tile.level - currentLevel);
            
            // Flat traversal
            if (levelDiff < 0.01f)
                return true;

            // Traversal from stair to stair
            if (tile.isStairs && GetTileAt(from).isStairs && levelDiff <= 0.3f)
                return true;
        }
        return false;
    }
    
    public GridTile GetTileAt(Vector2Int pos)
    {
        foreach (var tile in tiles)
        {
            if (tile.gridPos != pos || !tile.walkable)
                continue;
            return tile;
        }
        return null;
    }
}