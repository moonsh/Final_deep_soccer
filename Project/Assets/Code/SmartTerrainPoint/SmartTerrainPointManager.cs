using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class SmartTerrainPointManager
{
    static List<SmartTerrainPoint> availableSmartTerrainPointList = new List<SmartTerrainPoint>();
    static List<SmartTerrainPoint> smartTerrainPointsInUseList = new List<SmartTerrainPoint>();

    public static void RegisterSmartTerrainPoint(SmartTerrainPoint _terrainPoint) 
    {
        if (IsOnNavmesh(_terrainPoint))
        {
            availableSmartTerrainPointList.Add(_terrainPoint);
        }
        else Debug.LogWarningFormat("Terrain point {0} is not on navmesh and will be ignored", _terrainPoint.name);
    }

    public static SmartTerrainPoint SelectNearestReachableTerrainPoint(AIComponent _agent, out NavMeshPath _path)
    {
        SmartTerrainPoint validTerrainPoint = null;
        _path = null;

        availableSmartTerrainPointList.Sort
            ((x, y) => (x.transform.position - _agent.transform.position).sqrMagnitude.
            CompareTo((y.transform.position - _agent.transform.position).sqrMagnitude));

        foreach (SmartTerrainPoint _point in availableSmartTerrainPointList) 
        {
            if (IsValidTerrainPoint(_point, _agent, out _path)) 
            {
                validTerrainPoint = _point;
                validTerrainPoint.Reset();
                break;
            }
        }

        if (validTerrainPoint != null) 
        {
            availableSmartTerrainPointList.Remove(validTerrainPoint);
            smartTerrainPointsInUseList.Add(validTerrainPoint);
        }

        return validTerrainPoint;
    }

    static bool IsValidTerrainPoint(SmartTerrainPoint _terrainPoint, AIComponent _agent, out NavMeshPath _path) 
    {
        bool isValid = false;
        _path = null;

        if ( !_terrainPoint.IsOnCooldown() && _agent.behaviourTreeType == _terrainPoint.behaviourTreeType)
        {
            Vector3 distance = _terrainPoint.transform.position - _agent.GetPosition();
            if (distance.sqrMagnitude <= _terrainPoint.maxRange * _terrainPoint.maxRange)
            {
                if (IsOnNavmesh(_terrainPoint, out NavMeshHit _hit))
                {
                    _path = new NavMeshPath();
                    NavMesh.CalculatePath(_agent.GetPosition(), _hit.position, NavMesh.AllAreas, _path);

                    isValid = _path.status == NavMeshPathStatus.PathComplete;
                }
            }
        }

        return isValid;
    }

    static bool IsOnNavmesh(SmartTerrainPoint _terrainPoint, out NavMeshHit _hit)
    {
        _hit = default;

        bool isOnNavmesh = true;
        foreach (Transform _child in _terrainPoint.transform) 
        {
            if (!NavMesh.SamplePosition(_terrainPoint.transform.position, out _hit, 10, NavMesh.AllAreas))
            {
                isOnNavmesh = false;
                break;
            }
        }

        return isOnNavmesh;
    }

    static bool IsOnNavmesh(SmartTerrainPoint _terrainPoint) 
    {
        return IsOnNavmesh(_terrainPoint, out NavMeshHit _hit);
    }

    public static void OnSmartTerrainPointExit(SmartTerrainPoint _point) 
    {
        if (smartTerrainPointsInUseList.Contains(_point)) 
        {
            smartTerrainPointsInUseList.Remove(_point);
            availableSmartTerrainPointList.Add(_point);
        }
    }
}
