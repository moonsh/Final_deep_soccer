using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartTerrainPoint : MonoBehaviour
{
    public enum STPPathFollowType 
    {
        LOOP_PATH,
        EXIT_ON_PATH_END,
        STAY_ON_LAST_POINT
    }

    const string gizmoNameSTPActive = "TerrainPointIconActive.png";
    const string gizmoNameSTPInactive = "TerrainPointIconAvailable.png";
    const string gizmoNameCooldown = "TerrainPointIconCooldown.png";

    [Header("Settings")]
    public int maxRange = 10;
    public BehaviourTreeType behaviourTreeType;
    public float waitAtPathPointTime = 0;
    public float cooldownTime = 0;
    public STPPathFollowType pathFollowType;

    [Header("Animation Settings")]
    public string onPathPointReachedTriggerName;
    public string onPathEndTriggerName;

    
    List<Transform> terrainPathPoints;
    internal bool stpStarted = false;
    internal bool endPointReached = false;
    int nextPathPointIndex = 0;
    float cooldownTimer = 0;

    private void Awake()
    {
        terrainPathPoints = new List<Transform>(GetComponentsInChildren<Transform>());
        SmartTerrainPointManager.RegisterSmartTerrainPoint(this);
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        transform.position = new Vector3(transform.position.x, terrainHeight, transform.position.z);
    }

    private void Update()
    {
        if (cooldownTimer > 0) 
        {
            cooldownTimer -= Time.deltaTime;
        }
    }


    internal bool IsOnLastPoint()
    {
        return nextPathPointIndex == terrainPathPoints.Count - 1;
    }

    internal bool IsOnFirstPoint() { return nextPathPointIndex == 1; }

    public bool IsOnCooldown() { return cooldownTimer > 0; }

    public void OnPathPointReached() 
    {
        if (terrainPathPoints.Count != 0)
        {
            ++nextPathPointIndex;

            if (pathFollowType == STPPathFollowType.LOOP_PATH)
            {
                nextPathPointIndex = nextPathPointIndex % terrainPathPoints.Count;
            }
            else if (nextPathPointIndex >= terrainPathPoints.Count)
            {
                endPointReached = true;
                nextPathPointIndex = terrainPathPoints.Count - 1;
            }
        }
    }

    public Vector3 GetNextPathPoint()
    {
        return terrainPathPoints.Count == 0 ? Vector3.zero : terrainPathPoints[nextPathPointIndex].position; 
    }

    public void Reset()
    {
        stpStarted = false;
        nextPathPointIndex = 0;
        cooldownTimer = 0;
    }

    public void OnExit()
    {
        endPointReached = false;
        cooldownTimer = cooldownTime;
        stpStarted = false;
    }

    void OnDrawGizmos()
    {
        Transform[] terrainPoints = GetComponentsInChildren<Transform>();
        string gizmoName = cooldownTimer > 0 ? gizmoNameCooldown : stpStarted ? gizmoNameSTPActive : gizmoNameSTPInactive;
        Gizmos.DrawIcon(transform.position + Vector3.up * 0.5f, gizmoName, true);

        Gizmos.color = Color.green;

        for (int i = 0; i < terrainPoints.Length; ++i)
        {
            if (i == 0)
            {
                Gizmos.DrawLine(transform.position, terrainPoints[0].position);
            }
            else
            {
                Gizmos.DrawLine(terrainPoints[i - 1].position, terrainPoints[i].position);
            }

            Gizmos.DrawCube(terrainPoints[i].position, Vector3.one * 0.2f);
        }

        if (pathFollowType == STPPathFollowType.LOOP_PATH && terrainPoints.Length != 0)
        {
            Gizmos.DrawLine(terrainPoints[terrainPoints.Length - 1].position, transform.position);
        }
    }
}
