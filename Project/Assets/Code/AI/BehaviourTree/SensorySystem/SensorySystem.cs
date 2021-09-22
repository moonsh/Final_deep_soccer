using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class SensorySystem
{
    public float targetLostMaxTime;
    public float fovAngle = 60;
    public float fovDistance = 10;

    internal Vector3 lastKnownPosition = Vector3.zero;

    NavMeshAgent navAgent;
    AIComponent aiComponent;

    float targetLostTimer = 0;

    public void Initialize(AIComponent _aiComponent, NavMeshAgent _navAgent)
    {
        aiComponent = _aiComponent;
        navAgent = _navAgent;
    }

    internal void Update()
    {
        if (aiComponent.currentTarget != null)
        {
            UpdateHasTarget();
        }
    }

    private void UpdateHasTarget()
    {
        if (IsEventSourceVisible(aiComponent.currentTarget))
        {
            lastKnownPosition = aiComponent.currentTarget.GetPosition();
            targetLostTimer = 0;
        }
        else
        {
            targetLostTimer += Time.deltaTime;

            if (targetLostTimer >= targetLostMaxTime)
            {
                aiComponent.currentState = AIState.IDLE;
                navAgent.ResetPath();
                aiComponent.currentTarget = null;
                targetLostTimer = 0;
            }
        }
    }

    public bool IsEventSourceVisible(IEventSource _source)
    {
        bool isVisible = false;

        Vector3 sourcePosition = _source.GetPosition();
        Vector3 aiPosition = aiComponent.transform.position;

        aiPosition.y += 1;
        sourcePosition.y += 1;

        Vector3 vectorToSource = (sourcePosition - aiPosition);

        if (vectorToSource.sqrMagnitude < fovDistance * fovDistance)
        {
            Vector3 sourceDirection = vectorToSource.normalized;
            sourceDirection.y = 0;

            if (Vector3.Angle(aiComponent.transform.forward, sourceDirection) < (fovAngle / 2))
            {
                RaycastHit hit;
                if (Physics.Raycast(aiPosition, sourceDirection, out hit, fovDistance, ~LayerMask.GetMask("AI")))
                {
                    IEventSource componentHit = hit.collider.GetComponent<IEventSource>();
                    isVisible = componentHit == _source;
                }
            }
        }

        return isVisible;
    }
}
