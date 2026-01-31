using UnityEngine;
public class ClickToMove : MonoBehaviour
{
    public PlayerMovement player;
    public LayerMask floorLayer;
    
    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, floorLayer))
            return;
        
        Vector2Int target = new Vector2Int(
            Mathf.RoundToInt(hit.point.x),
            Mathf.RoundToInt(hit.point.z)
        );
        
        Vector2Int start = player.CurrentGridPos();
        var path = Pathfinder.FindPath(start, target, player.playerFloor); // Pass playerFloor
        
        if (path != null)
            player.MoveAlongPath(path);
    }
}