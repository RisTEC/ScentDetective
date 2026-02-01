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
    public Dictionary<string,string> characterSmells = new Dictionary<string, string>{
        {"Savory Spice","Chef"},
        {"Terra Silva","Gardener"},
        {"Timber","Lumberjack"},
        {"Machina","Mechanic"},
        {"Floral","Millionaire"},
        {"Kindred","Nanny"}};
    private float smellRange = 20.0f;
    public SelectedScentUI scentUI;

    void Start()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        SelectedScent = listOfScents[CurrentScent];
        UpdateScentUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CurrentScent = (CurrentScent + 1) % listOfScents.Count;
            SelectedScent = listOfScents[CurrentScent];
            UpdateScentUI();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CurrentScent = (CurrentScent - 1 + listOfScents.Count) % listOfScents.Count;
            SelectedScent = listOfScents[CurrentScent];
            UpdateScentUI();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Sniff();
        }
    }
    
    void UpdateScentUI()
    {
        if (scentUI != null)
        {
            scentUI.SelectScent(SelectedScent.name);
        }
    }
    
    public void Sniff()
    {
        int itemIndex = -1;
        int minDistance = int.MaxValue;
        List<Vector2Int> itemPath = null;

        for (int i = 0; i < ItemManager.Instance.Items.Count; i++)
        {   
            if(ItemManager.Instance.Items[i].discovered)
                break;
            if (ItemManager.Instance.Items[i].scent == SelectedScent)
            {
                List<Vector2Int> path = Pathfinder.FindPath(
                    player.CurrentGridPos(),
                    ItemManager.Instance.Items[i].gridPos,
                    player.playerFloor,
                    true);

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

    public void CreateTrail(List<Vector2Int> itemPath, int itemIndex, float intensity)
    {
        Debug.Log("Spawning Trail with intensity: " + intensity);
        if (itemPath.Count > 3)
        {
            Trail trail = Instantiate(TrailPrefab, Vector3.zero, Quaternion.identity).GetComponent<Trail>();
            trail.path = itemPath;
            trail.currentIndex = 1;
            trail.trailLength = 4+2;
            trail.intensity = intensity;
            trail.SetForTile(trail.currentIndex);
            trail.UpdateIntensity(false);
            trail.StartTrail();
        }
        else
        {
            Trail trail = Instantiate(TrailPrefab, Vector3.zero, Quaternion.identity).GetComponent<Trail>();
            trail.path = itemPath;
            trail.currentIndex = 0;
            trail.trailLength = 0;
            trail.intensity = intensity * 1.5f;
            trail.FaceTarget(ItemManager.Instance.Items[itemIndex].transform.position, player.transform.position);
            trail.UpdateIntensity(true);
            trail.StartTrail();
        }
    }
}