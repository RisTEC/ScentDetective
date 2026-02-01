using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using OVR.Data;
using OVR.Components;
using System;


public class ScentManager : MonoBehaviour
{
    public List<OdorAsset> listOfScents;
    public static ScentManager Instance;
    public PlayerMovement player;
    public int CurrentScent = 0;
    public OdorAsset SelectedScent;

    private float smellRange = 15.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;

        SelectedScent = listOfScents[CurrentScent];
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (CurrentScent < listOfScents.Count)
            {
                CurrentScent++;
                SelectedScent = listOfScents[CurrentScent];
            }
            else
            {
                CurrentScent = listOfScents.Count;
            }

        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (CurrentScent > 0)
            {
                CurrentScent--;
                SelectedScent = listOfScents[CurrentScent];
            }
            else
            {
                CurrentScent = 0;
            }
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
            if (ItemManager.Instance.Items[i].scent == SelectedScent)
            {
                // Find path from player to item
                List<Vector2Int> path = Pathfinder.FindPath(
                player.CurrentGridPos(),
                ItemManager.Instance.Items[i].gridPos,
                player.playerFloor,
                true);

                // Set new closest item
                if (path != null && path.Count < minDistance)
                {
                    minDistance = path.Count;
                    itemIndex = i;
                    itemPath = path;
                }
            }

            Debug.Log(minDistance);
            if (minDistance != int.MaxValue)
            {
                OlfactoryEpithelium.Get().PlayOdor(SelectedScent, Mathf.Ceil((minDistance/smellRange)*4.0f)*(255.0f/4.0f));
            }

        }
    }


    public void CreateTrail()
    {

    }
}
