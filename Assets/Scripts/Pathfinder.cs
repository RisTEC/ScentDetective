using System.Collections.Generic;
using UnityEngine;
public static class Pathfinder
{
    static readonly Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,
        new(1,1),
        new(-1,1),
        new(-1,-1),
        new(1,-1)
    };
    
    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal, float startLevel, bool adjacentToGoal)
    {
        Debug.Log($"=== PATHFINDING START ===");
        Debug.Log($"Start: {start}, Goal: {goal}, StartLevel: {startLevel:F2}");
        
        Queue<(Vector2Int pos, float level)> frontier = new Queue<(Vector2Int, float)>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        
        frontier.Enqueue((start, startLevel));
        cameFrom[start] = start;
        
        int iterations = 0;
        int maxLoggedSteps = 15; // Only log first 15 steps to avoid spam
        
        Vector2Int lastTile = goal;

        while (frontier.Count > 0)
        {
            iterations++;
            var (current, currentLevel) = frontier.Dequeue();
            lastTile = current;
            
            if (iterations <= maxLoggedSteps)
                Debug.Log($"[{iterations}] Exploring {current} at level {currentLevel:F2}");
            
            if (current == goal)
            {
                Debug.Log($"<color=green>GOAL REACHED at {current} in {iterations} iterations!</color>");
                break;
            }
            
            foreach (var dir in directions)
            {
                var next = current + dir;
                
                if( adjacentToGoal && next == goal)
                {
                    // End search if adjacent to the goal
                    Debug.Log($"<color=green>GOAL REACHED at {current} in {iterations} iterations!</color>");
                    frontier = new Queue<(Vector2Int, float)>();
                    break;
                }

                // Tile already covered by search
                if (cameFrom.ContainsKey(next))
                    continue;
                
                bool walkable = GridManager.Instance.IsWalkable(next, currentLevel, current);
                
                // Obstacle blocking tile, exlude it
                if (!walkable)
                {
                    if (iterations <= maxLoggedSteps)
                        Debug.Log($"  {next} - Not walkable from level {currentLevel:F2}");
                    continue;
                }
                
                GridTile nextTile = GridManager.Instance.GetTileAt(next);
                
                if (nextTile == null)
                {
                    if (iterations <= maxLoggedSteps)
                        Debug.LogWarning($"  {next} - IsWalkable=true but GetTileAt returned null!");
                    continue;
                }
                
                float nextLevel = nextTile.transform.position.y;
                
                if (iterations <= maxLoggedSteps)
                    Debug.Log($"  {next} - level={nextLevel:F2}, isStairs={nextTile.isStairs}");
                
                frontier.Enqueue((next, nextLevel));
                cameFrom[next] = current;
            }
            
            if (iterations > 1000)
            {
                Debug.LogError("Pathfinding exceeded 1000 iterations - infinite loop protection!");
                return null;
            }
        }
        
        if (!cameFrom.ContainsKey(goal) && !adjacentToGoal)
        {
            Debug.LogWarning($"<color=red>NO PATH FOUND after {iterations} iterations</color>");
            return null;
        }
        
        Debug.Log("Reconstructing path");
        // Reconstruct path
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int temp = lastTile;
        while (temp != start)
        {
            path.Add(temp);
            temp = cameFrom[temp];
        }
        path.Reverse();
        
        Debug.Log($"<color=green>Path found with {path.Count} steps:</color>");
        string pathStr = "";
        foreach (var p in path)
            pathStr += $"{p} -> ";
        Debug.Log(pathStr + "GOAL");
        
        return path;
    }
}