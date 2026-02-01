using UnityEngine;

public class ClickItem : MonoBehaviour
{
    public PlayerMovement player;
    public LayerMask itemLayer;
    public Item currentItem;

    void Update()
    {
        Ray rayItem = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit itemHit;

        if (!Physics.Raycast(rayItem, out itemHit, 100f, itemLayer))
        {
            currentItem = null;
            return;
        }

        currentItem = itemHit.collider.GetComponentInParent<Item>();

        if (!Input.GetMouseButtonDown(0))
            return;

        if (currentItem != null)
        {
            float dist = Vector3.Distance(
                player.transform.position,
                currentItem.transform.position
            );

            if (dist <= 1.5f)
            {
                Vector3 dir = currentItem.transform.position - player.transform.position;
                dir.y = 0f;

                if (dir.sqrMagnitude > 0.001f)
                {
                    player.transform.rotation = Quaternion.LookRotation(dir);
                }

                currentItem.Interact();
            }
        }
    }
}
