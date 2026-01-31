using UnityEngine;

public class GridTile : MonoBehaviour
{
    public Vector2Int gridPos;
    public bool walkable = true;
    public int level = 0;

    void Awake()
    {
        gridPos = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.z)
        );
        level = Mathf.RoundToInt(transform.position.y);
    }
}
