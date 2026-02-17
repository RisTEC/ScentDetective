using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
public class GridManager : MonoBehaviour
{
    public List<GridTile> tiles = new List<GridTile>();
    public static GridManager Instance;
    public float stairHeight = 0.5f;
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

        foreach (GridTile tile in tiles)
        {
            tile.UpdateGridInfo();

        }
    }

    public bool IsWalkable(Vector2Int target, float currentLevel, Vector2Int from)
    {
        GridTile fromTile = GetTileAt(from);
        foreach (var tile in tiles)
        {
            if (tile.gridPos != target || !tile.walkable)
                continue;

            float levelDiff = Mathf.Abs(tile.transform.position.y - currentLevel);

            // Only move orthogonally on stairs
            if(fromTile.isStairs && tile.isStairs && target.x != from.x && target.y != from.y)
            {
                continue;
            }

            // Flat traversal - same level only
            if (levelDiff < 0.3f)
                return true;

            // Level change traversal - BOTH tiles must be stairs
            if (fromTile != null && tile.isStairs && fromTile.isStairs && levelDiff <= stairHeight)
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