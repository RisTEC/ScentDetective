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
    
    public bool IsWalkable(Vector2Int pos, float currentLevel)
    {
        foreach (var tile in tiles)
        {
            if (tile.gridPos != pos || !tile.walkable)
                continue;
            
            float levelDiff = Mathf.Abs(tile.level - currentLevel);
            
            if (levelDiff < 0.01f)
                return true;
            
            if (tile.isStairs && levelDiff <= 0.55f)
                return true;
        }
        return false;
    }
    
    public GridTile GetTileAt(Vector2Int pos, float currentLevel)
    {
        foreach (var tile in tiles)
        {
            if (tile.gridPos != pos || !tile.walkable)
                continue;
            
            float levelDiff = Mathf.Abs(tile.level - currentLevel);
            
            if (levelDiff < 0.01f || (tile.isStairs && levelDiff <= 0.55f))
                return tile;
        }
        return null;
    }
}