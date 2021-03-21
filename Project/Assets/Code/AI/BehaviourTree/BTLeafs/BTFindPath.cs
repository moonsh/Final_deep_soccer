using UnityEngine;
using UnityEngine.AI;

[BTAgent(typeof(BTFindPath))]
public class BTFindPath : BTNode
{
    public PathType pathType;

    public float repathTolerance = 2;
    public float repathCount = 10;
    public float minWanderDistance = 10;
    public float maxWanderDistance = 15;

    public override BTResult Execute()
    {
        switch (pathType)
        {
            case PathType.TARGET:
                if (context.contextOwner.currentTarget != null && !context.navAgent.pathPending && context.navAgent.enabled)
                {
                    SetDestinationNearTarget();
                }
                break;
            case PathType.RANDOM:
                if (!context.navAgent.pathPending && context.navAgent.enabled)
                {
                    SetRandomDestination(context.navAgent);
                }
                break;
            default:
                break;
        }

        return context.navAgent.hasPath || context.navAgent.pathPending ? BTResult.SUCCESS : BTResult.FAILURE;
       
    }

    public void SetRandomDestination(NavMeshAgent _agent)
    {
        float radius = Random.Range(minWanderDistance, maxWanderDistance);
        Vector3 randomPosition = Random.insideUnitSphere * radius;
        randomPosition += _agent.transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, radius, 1))
        {
            _agent.SetDestination(hit.position);
        }
    }

    //Tries RepathCount times to find a path near the player, increasing the radius each time. 
    //This is needed in case the player is not on valid Navmesh
    public void SetDestinationNearTarget()
    {
        NavMeshHit hit;
        float radius = 0;
        for (int i = 0; i < repathCount; ++i)
        {
            Vector3 randomPosition = Random.insideUnitSphere * radius;
            randomPosition += context.contextOwner.currentTarget.GetPosition();
            if (NavMesh.SamplePosition(randomPosition, out hit, radius, 1))
            {
                context.navAgent.SetDestination(hit.position);
                break;
            }
            else
            {
                ++radius;
            }
        }
    }
}
