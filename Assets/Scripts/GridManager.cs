using System.Collections.Generic;
using UnityEngine;
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public List<GridTile> allTiles = new List<GridTile>();
    
    void Start()
    {
        Instance = this;
        GridTile[] foundTiles = FindObjectsByType<GridTile>(FindObjectsSortMode.None);
        allTiles.AddRange(foundTiles);
        
        int stairCount = 0;
        foreach (var tile in allTiles)
        {
            if (tile.isStairs)
            {
                stairCount++;
            }
        }
    }
    
    public bool IsWalkable(Vector2Int pos, float currentLevel)
    {
        foreach (var tile in allTiles)
        {
            if (tile.gridPos != pos || !tile.walkable)
                continue;
            
            float levelDiff = Mathf.Abs(tile.level - currentLevel);
            
            if (levelDiff < 0.01f)
                return true;
            
            if (tile.isStairs && levelDiff <= 0.3f)
                return true;
        }
        return false;
    }
    
    public GridTile GetTileAt(Vector2Int pos, float currentLevel)
    {
        foreach (var tile in allTiles)
        {
            if (tile.gridPos != pos || !tile.walkable)
                continue;
            
            float levelDiff = Mathf.Abs(tile.level - currentLevel);
            
            if (levelDiff < 0.01f || (tile.isStairs && levelDiff <= 0.3f))
                return tile;
        }
        return null;
    }
}