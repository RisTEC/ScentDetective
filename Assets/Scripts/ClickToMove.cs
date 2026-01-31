using UnityEngine;
public class ClickToMove : MonoBehaviour
{
    public PlayerMovement player;
    public LayerMask floorLayer;
    public GridTile current;

    void Start()
    {
        floorLayer = LayerMask.GetMask("Floor");
    }
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, 100f, floorLayer))
        {
            if (current != null)
            {
                current.Unhighlight();
                current = null;
            }
            return;
        }
        if (current != null)
        {
            current.Unhighlight();
            current = null;
        }
        current = hit.collider.GetComponentInParent<GridTile>();
        if (current.walkable)
        {
            current.Highlight();
        }
        Vector2Int target = new Vector2Int(
            Mathf.RoundToInt(hit.point.x),
            Mathf.RoundToInt(hit.point.z)
        );

        if (!Input.GetMouseButtonDown(0))
            return;
        else
            Debug.Log("Left Click");

        Vector2Int start = player.CurrentGridPos();
        var path = Pathfinder.FindPath(start, target, player.playerFloor);

        if (path != null){
            Debug.Log("Assigning Path");
            player.MoveAlongPath(path);
        }
    }
}