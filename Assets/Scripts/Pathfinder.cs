using System.Collections.Generic;
using UnityEngine;
public static class Pathfinder
{
    static readonly Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };
    
    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal, float startLevel)
    {
        Queue<(Vector2Int pos, float level)> frontier = new Queue<(Vector2Int, float)>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        
        frontier.Enqueue((start, startLevel));
        cameFrom[start] = start;
        
        while (frontier.Count > 0)
        {
            var (current, currentLevel) = frontier.Dequeue();
            
            if (current == goal)
                break;
            
            foreach (var dir in directions)
            {
                var next = current + dir;
                
                if (cameFrom.ContainsKey(next))
                    continue;
                
                if (!GridManager.Instance.IsWalkable(next, currentLevel))
                    continue;
                
                GridTile nextTile = GridManager.Instance.GetTileAt(next, currentLevel);
                float nextLevel = nextTile != null ? nextTile.level : currentLevel;
                
                frontier.Enqueue((next, nextLevel));
                cameFrom[next] = current;
            }
        }
        
        if (!cameFrom.ContainsKey(goal))
            return null;
        
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int temp = goal;
        while (temp != start)
        {
            path.Add(temp);
            temp = cameFrom[temp];
        }
        path.Reverse();
        return path;
    }
}