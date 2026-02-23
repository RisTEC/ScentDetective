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
    public int scentTrailLength = 4;
    public Dictionary<string, string> characterSmells = new Dictionary<string, string>{
        {"Savory Spice","Chef"},
        {"Terra Silva","Gardener"},
        {"Timber","Lumberjack"},
        {"Machina","Mechanic"},
        {"Floral","Millionaire"},
        {"Kindred","Nanny"}};
    public Dictionary<string, Color> scentColors;
    private float smellRange = 20.0f;
    public SelectedScentUI scentUI;

    void Start()
    {
        scentColors = new Dictionary<string, Color>
            {
                {"Savory Spice", StringToColor("#A8632C")},
                {"Terra Silva", StringToColor("#677166")},
                {"Timber", StringToColor("#815D4D")},
                {"Machina", StringToColor("#333333")},
                {"Floral", StringToColor("#E87D9B")},
                {"Kindred", StringToColor("#B399C9")}
            };
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        SelectedScent = listOfScents[CurrentScent];
        UpdateScentUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            IncrementSelectedScent(-1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            IncrementSelectedScent(1);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Sniff();
        }
    }
    private Color StringToColor(string text)
    {
        if(UnityEngine.ColorUtility.TryParseHtmlString(text, out Color color))
        {
            return color;
        }
        Debug.LogWarning("Could not interpret color");
        return Color.white;
    }
    public void IncrementSelectedScent(int amount)
    {
        CurrentScent = (CurrentScent + amount + listOfScents.Count) % listOfScents.Count;
        SelectedScent = listOfScents[CurrentScent];
        UpdateScentUI();
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
        Debug.Log("Sniffing");
        int itemIndex = -1;
        int minDistance = int.MaxValue;
        List<Vector2Int> itemPath = null;

        for (int i = 0; i < ItemManager.Instance.Items.Count; i++)
        {   
            if (ItemManager.Instance.Items[i].scent == SelectedScent && !ItemManager.Instance.Items[i].discovered)
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
                    itemPath.Add(ItemManager.Instance.Items[i].gridPos);
                }
            }
        }
        if (itemPath != null && itemPath.Count <= smellRange)
        {
            CreateTrail(itemPath, itemIndex, Mathf.Pow(1 - minDistance / smellRange, 2) * 2.0f);
            OlfactoryEpithelium.Get().PlayOdor(SelectedScent, Mathf.Ceil((1 - minDistance / smellRange) * 4.0f) * (255.0f / 4.0f));
        }
    }

    public void CreateTrail(List<Vector2Int> itemPath, int itemIndex, float intensity)
    {
        Debug.Log("Spawning Trail with intensity: " + intensity);
        if (itemPath.Count > 2)
        {
            // Follow path
            Trail trail = Instantiate(TrailPrefab, Vector3.zero, Quaternion.identity).GetComponent<Trail>();
            trail.path = itemPath;
            trail.currentIndex = 1;
            trail.trailLength = scentTrailLength;
            trail.intensity = intensity;
            trail.SetForTile(trail.currentIndex);
            trail.UpdateIntensity(false);
            trail.StartTrail();
        }
        else
        {
            // Directly to player
            Trail trail = Instantiate(TrailPrefab, Vector3.zero, Quaternion.identity).GetComponent<Trail>();
            trail.path = itemPath;
            trail.currentIndex = 0;
            trail.trailLength = 0;
            trail.intensity = intensity * 1.5f;
            trail.FaceTarget(ItemManager.Instance.Items[itemIndex].transform.position, ItemManager.Instance.Items[itemIndex].transform.position + Vector3.up);
            trail.UpdateIntensity(true);
            trail.StartTrail();
        }
    }
}