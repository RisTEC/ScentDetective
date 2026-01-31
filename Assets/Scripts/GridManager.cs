using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public Dictionary<Vector2Int, GridTile> tiles =
        new Dictionary<Vector2Int, GridTile>();

    void Start()
    {
        Instance = this;

        GridTile[] allTiles =
            FindObjectsByType<GridTile>(FindObjectsSortMode.None);

        foreach (var tile in allTiles)
        {
            tiles[tile.gridPos] = tile;
        }
    }

    public bool IsWalkable(Vector2Int pos)
    {
        return tiles.ContainsKey(pos) && tiles[pos].walkable;
    }
}
