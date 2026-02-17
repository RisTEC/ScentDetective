using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class Trail : MonoBehaviour
{
    public List<Vector2Int> path;
    public int currentIndex = 1;
    public int trailLength = 3; //default; is assigned in scent manager
    public float intensity = 1.0f;
    private ParticleSystem ps;
    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    /// <summary>
    /// Set position and rotation according to an index of the path
    /// </summary>
    /// <param name="index"></param>
    public void SetForTile(int index)
    {
        Vector3 spawnPosition = GridManager.Instance.GetTileAtUnsafe(path[index]).transform.position;
        Vector3 targetPosition = GridManager.Instance.GetTileAtUnsafe(path[index - 1]).transform.position;
        transform.SetPositionAndRotation(spawnPosition + new Vector3(0, 1, 0), Quaternion.LookRotation(targetPosition - spawnPosition, Vector3.up));
    }
    /// <summary>
    /// Directly set the position and roation
    /// </summary>
    /// <param name="spawn"></param>
    /// <param name="target"></param>
    public void FaceTarget(Vector3 spawn, Vector3 target)
    {
        transform.position = spawn;
        transform.rotation = Quaternion.LookRotation(target - spawn, Vector3.up);
    }
    /// <summary>
    /// Update particle spawner settings according to intensity
    /// </summary>
    public void UpdateIntensity(bool longer)
    {
        ParticleSystem.MainModule mainSettings = ps.main;
        mainSettings.startSizeMultiplier = intensity * 1.5f + 0.5f;
        if (longer)
        {
            mainSettings.duration = 2.0f;
        }
        ParticleSystem.EmissionModule emmisionSettings = ps.emission;
        ParticleSystem.MinMaxCurve rate = emmisionSettings.rateOverTime;
        rate.constant = 5 * (intensity * 1.5f + 0.5f);
        emmisionSettings.rateOverTime = rate;
    }
    void Update()
    {
        // triggers in between emmissions for each tile
        if (!ps.isEmitting)
        {
            if (currentIndex >= path.Count || currentIndex > trailLength || currentIndex == 0)
            {
                // Either too close to item or done with trail
                if (ps.isStopped)
                    Destroy(gameObject);
            }
            else
            {
                // Move spawner to next tile in path
                SetForTile(currentIndex);
                ps.Play();
                currentIndex++;
            }
        }
    }
    public void StartTrail()
    {
        ps.Play();
    }
}
