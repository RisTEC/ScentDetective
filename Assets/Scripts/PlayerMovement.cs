using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float playerFloor = 0f;
    public float heightOffset = 0.8f;
    Queue<Vector3> worldPath = new Queue<Vector3>();
    
    void Update()
    {
        if (worldPath.Count == 0)
            return;
        
        Vector3 target = worldPath.Peek();
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            moveSpeed * Time.deltaTime
        );
        
        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            transform.position = target;
            worldPath.Dequeue();
            
            playerFloor = (transform.position.y - heightOffset) / 3f;
        }
    }
    
    public void MoveAlongPath(List<Vector2Int> path)
    {
        worldPath.Clear();
        float currentLevel = playerFloor;
        
        foreach (var p in path)
        {
            GridTile tile = GridManager.Instance.GetTileAt(p, currentLevel);
            
            if (tile != null)
            {
                float yPos = (tile.level * 3f) + heightOffset;
                worldPath.Enqueue(new Vector3(p.x, yPos, p.y));
                
                currentLevel = tile.level;
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