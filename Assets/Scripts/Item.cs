using UnityEngine;

public class Item : MonoBehaviour
{
    // Set this to true when it is not located above a tile; manually set the below information in inspector
    public bool ManualLocation;
    public Vector2Int gridPos;
    public float level;

    /// <summary>
    /// Raycast to find a matching tile location
    /// </summary>
    /// <returns></returns>
    public bool AssignLocation()
    {
        // Raycast down to find a matching gridTile
        RaycastHit hitInfo;
        Debug.DrawRay(transform.position, Vector3.down, Color.blue, 10.0f);
        if (Physics.Raycast(
            transform.position, Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Floor")))
        {
            // Copy gridTile info over
            GridTile hitTile = hitInfo.collider.GetComponent<GridTile>();
            gridPos = hitTile.gridPos;
            level = hitTile.level;
            return true;
        }
        else
        {
            // Default info because there is no tile
            gridPos = Vector2Int.zero;
            level = 0;
            return false;
        }
    }
}
