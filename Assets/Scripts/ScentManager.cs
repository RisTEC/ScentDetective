using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using OVR.Data;
using OVR.Components;
using System;
using Unity.Mathematics;


public class ScentManager : MonoBehaviour
{
    public List<OdorAsset> listOfScents;
    public static ScentManager Instance;
    private PlayerMovement player;
    public int CurrentScent = 0;
    public OdorAsset SelectedScent;
    public GameObject TrailPrefab;

    private float smellRange = 20.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        SelectedScent = listOfScents[CurrentScent];
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CurrentScent = (CurrentScent + 1) % listOfScents.Count;
            SelectedScent = listOfScents[CurrentScent];

        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CurrentScent = (CurrentScent - 1 + listOfScents.Count) % listOfScents.Count;
            SelectedScent = listOfScents[CurrentScent];
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
        List<Vector2Int> itemPath = null;

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
        }
        if (itemPath != null && itemPath.Count <= smellRange)
        {
            CreateTrail(itemPath, itemIndex, Mathf.Pow(1 - minDistance / smellRange,2)*2.0f);
            OlfactoryEpithelium.Get().PlayOdor(SelectedScent, Mathf.Ceil((1 - minDistance / smellRange) * 4.0f) * (255.0f / 4.0f));
        }
    }

    /// <summary>
    /// Spawns particles to indicate the direction of an item
    /// </summary>
    /// <param name="itemPath"></param>
    /// <param name="itemIndex"></param>
    /// <param name="intensity"></param>
    public void CreateTrail(List<Vector2Int> itemPath, int itemIndex, float intensity)
    {
        Debug.Log("Spawning Trail with intensity: " + intensity);
        if (itemPath.Count > 3)
        {
            // Player is farther away, create a trail
            Trail trail = Instantiate(TrailPrefab, Vector3.zero, Quaternion.identity).GetComponent<Trail>();
            trail.path = itemPath;
            trail.currentIndex = 1;
            trail.trailLength = 4+2;
            trail.intensity = intensity;
            trail.SetForTile(trail.currentIndex);
            trail.UpdateIntensity(false);
        }
        else
        {
            // Player is close to item, create scent direction from item
            Trail trail = Instantiate(TrailPrefab, Vector3.zero, Quaternion.identity).GetComponent<Trail>();
            trail.path = itemPath;
            trail.currentIndex = 0;
            trail.trailLength = 0;
            trail.intensity = 5;
            trail.FaceTarget(ItemManager.Instance.Items[itemIndex].transform.position, player.transform.position);
            trail.UpdateIntensity(true);
        }
    }
}
