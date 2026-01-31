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

    public static List<Vector2Int> FindPath(
        Vector2Int start,
        Vector2Int goal)
    {
        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom =
            new Dictionary<Vector2Int, Vector2Int>();

        frontier.Enqueue(start);
        cameFrom[start] = start;

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current == goal)
                break;

            foreach (var dir in directions)
            {
                var next = current + dir;

                if (!GridManager.Instance.IsWalkable(next))
                    continue;

                if (cameFrom.ContainsKey(next))
                    continue;

                frontier.Enqueue(next);
                cameFrom[next] = current;
            }
        }

        if (!cameFrom.ContainsKey(goal))
            return null;

        // Reconstruct path
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
