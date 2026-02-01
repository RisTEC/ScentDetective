using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Trail : MonoBehaviour
{
    public List<Vector2Int> path;
    public int currentIndex = 1;
    public int trailLength = 3; //default; is assigned in scent manager
    public float intensity = 1.0f;
    private ParticleSystem ps;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }
    /// <summary>
    /// Set position and rotation according to an index of the path
    /// </summary>
    /// <param name="index"></param>
    public void SetForTile(int index)
    {
        Vector3 spawnPosition = GridManager.Instance.GetTileAt(path[index + 2]).transform.position;
        Vector3 targetPosition = GridManager.Instance.GetTileAt(path[index]).transform.position;
        transform.position = spawnPosition + new Vector3(0, 1, 0);
        transform.rotation = Quaternion.LookRotation(targetPosition - spawnPosition, Vector3.up);
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
            if (currentIndex + 2 >= path.Count || currentIndex + 2 >= trailLength)
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
}
