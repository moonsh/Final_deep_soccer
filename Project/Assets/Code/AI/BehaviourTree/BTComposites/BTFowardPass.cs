using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTFowardPass : BTNode
{
    public override BTResult Execute()
    {
        HashSet<GameObject> cannotPassAgents = new HashSet<GameObject>();
        RaycastHit hit;
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        foreach (GameObject teammate in context.teammates)
        {
            Vector3 direction_to_agent;
            direction_to_agent = teammate.transform.position - context.navAgent.transform.position;
            direction_to_agent = direction_to_agent.normalized;
            if (Physics.Raycast(context.navAgent.transform.position, direction_to_agent, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider != null)
                {
                    cannotPassAgents.Add(teammate);
                    Debug.DrawRay(context.navAgent.transform.position, direction_to_agent * hit.distance, Color.yellow);
                }
            }
        }
        float distance1 = 1000;
        Vector3 teammatePos = new Vector3(0, 0, 0);
        foreach(GameObject teammate in context.teammates)
        {
            if(!cannotPassAgents.Contains(teammate))
            {
                float distance2 = Mathf.Sqrt(((teammate.transform.position.z - context.navAgent.transform.position.z) * (teammate.transform.position.z - context.navAgent.transform.position.z))
                        + ((teammate.transform.position.x - context.navAgent.transform.position.x) * (teammate.transform.position.x - context.navAgent.transform.position.x)));
                if(distance2< distance1)
                {
                    distance1 = distance2;
                    teammatePos = teammate.transform.position;
                }
            }
        }
        context.navAgent.SetDestination(teammatePos);
        if (Vector3.Angle(context.navAgent.transform.forward, teammatePos - context.navAgent.transform.position) < 10)
        {
            Debug.Log(100);
            context.navAgent.GetComponent<AgentSoccer>().Kick(teammatePos, 170f * distance1);
        }
        return BTResult.SUCCESS;
    }
}
