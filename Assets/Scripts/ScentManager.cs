using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Scent
{
    None,
    Suspect1,
    Suspect2,
    Suspect3,
    Suspect4,
    Suspect5

}
public class ScentSelector : MonoBehaviour
{
    public Scent SelectedScent;
    public static ScentSelector Instance;
    private PlayerMovement player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        SelectedScent = Scent.None;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SelectedScent = Scent.None;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectedScent = Scent.Suspect1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectedScent = Scent.Suspect2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectedScent = Scent.Suspect3;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Sniff();
        }
    }
    public void Sniff()
    {
        // Get closest item with selected scent
        int itemIndex = -1;
        int minDistance = int.MaxValue;
        List<Vector2Int> itemPath;

        for (int i = 0; i < ItemManager.Instance.Items.Count; i++)
        {
            // Find path from player to item
            List<Vector2Int> path = Pathfinder.FindPath(
                player.CurrentGridPos(),
                ItemManager.Instance.Items[i].gridPos,
                player.playerFloor);
            
            // Set new closest item
            if(path.Count < minDistance)
            {
                minDistance = path.Count;
                itemIndex = i;
                itemPath = path;
            }
        }


    }

    public void CreateTrail()
    {
        
    }
}
