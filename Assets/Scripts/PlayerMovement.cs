using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float playerFloor = 0f;
    public float heightOffset = 1f;
    Queue<Vector3> worldPath = new Queue<Vector3>();

    void Update()
    {
        if (worldPath.Count == 0)
            return;

        Vector3 target = worldPath.Peek();

        // Direction to next tile (ignore Y)
        Vector3 dir = target - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                10f * Time.deltaTime
            );
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            transform.position = target;
            worldPath.Dequeue();

            playerFloor = Mathf.Round((transform.position.y - heightOffset) / 3f * 4f) / 4f;
        }
    }

    public void MoveAlongPath(List<Vector2Int> path)
    {
        worldPath.Clear();

        foreach (var p in path)
        {
            GridTile tile = GridManager.Instance.GetTileAt(p);
            if (tile != null)
            {
                float yPos = (tile.level * 3f) + heightOffset;
                worldPath.Enqueue(new Vector3(p.x, yPos, p.y));
            }
        }
    }

    public Vector2Int CurrentGridPos()
    {
        return new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.z)
        );
    }
}