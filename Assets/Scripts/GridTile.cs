using UnityEngine;
public class GridTile : MonoBehaviour
{
    public Vector2Int gridPos;
    public bool walkable = true;
    public float level = 0; 
    public bool isStairs = false; 
    
    void Awake()
    {
        gridPos = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.z)
        );
        level = Mathf.Round(transform.position.y / 2.4f * 4f) / 4f;
    }
}