using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTFowardPass : BTNode
{
    public override BTResult Execute()
    {
        HashSet<GameObject> cannotPassAgents = new HashSet<GameObject>();
        RaycastHit hit;
        
        //check barrier-free teammates that are in front of the agent
        bool passback = true;
        foreach (GameObject teammate in context.teammates)
        {
            Vector3 direction_to_agent;
            direction_to_agent = teammate.transform.position - context.navAgent.transform.position;
            direction_to_agent = direction_to_agent.normalized;
            if((context.navAgent.tag == "blueAgent"&& teammate.transform.position.z<context.navAgent.transform.position.z)
                ||(context.navAgent.tag == "purpleAgent" && teammate.transform.position.z > context.navAgent.transform.position.z))
            {
                if (Physics.Raycast(context.navAgent.transform.position, direction_to_agent, out hit, Mathf.Infinity))
                {
                    if (hit.collider.tag != context.navAgent.tag)
                    {   
                        cannotPassAgents.Add(teammate);
                    }
                    else
                    {
                        passback = false;
                    }

                }
            }
            else
            {
                cannotPassAgents.Add(teammate);
            }
        }
        if (passback)
        {
            cannotPassAgents = new HashSet<GameObject>();
            //check barrier free teammates that are behind of the agent
            foreach (GameObject teammate in context.teammates)
            {
                Vector3 direction_to_agent;
                direction_to_agent = teammate.transform.position - context.navAgent.transform.position;
                direction_to_agent = direction_to_agent.normalized;
                if (Physics.Raycast(context.navAgent.transform.position, direction_to_agent, out hit, Mathf.Infinity))
                {
                    if (hit.collider.tag != context.navAgent.tag)
                    {
                        cannotPassAgents.Add(teammate);
                        Debug.DrawRay(context.navAgent.transform.position, direction_to_agent * hit.distance, Color.yellow);
                    }

                }
            }
        }
        
        bool goBack = true;
        float distance1 = 1000;
        Vector3 teammatePos = new Vector3(0, 0, 0);
        foreach(GameObject teammate in context.teammates)
        {
            if(!cannotPassAgents.Contains(teammate))
            {
                goBack = false;
                float distance2 = Mathf.Sqrt(((teammate.transform.position.z - context.navAgent.transform.position.z) * (teammate.transform.position.z - context.navAgent.transform.position.z))
                        + ((teammate.transform.position.x - context.navAgent.transform.position.x) * (teammate.transform.position.x - context.navAgent.transform.position.x)));
                if(distance2 < distance1)
                {
                    distance1 = distance2;
                    teammatePos = teammate.transform.position;
                }
            }
        }
        if(goBack)
        {
            if(context.navAgent.tag == "blueAgent")
            {
                context.navAgent.SetDestination(context.navAgent.transform.position + new Vector3(0, 0,1));
            }
            else
            {
                context.navAgent.SetDestination(context.navAgent.transform.position - new Vector3(0, 0, 1));
            }
        }
        else
        {
            Debug.DrawLine(context.navAgent.transform.position,teammatePos,Color.red, 1f);
            context.navAgent.SetDestination(teammatePos);
        }
        if (Vector3.Angle(context.navAgent.transform.forward, teammatePos - context.navAgent.transform.position) < 15)
        {
            
            if(context.navAgent.speed < 5)
            {
                context.navAgent.GetComponent<AgentSoccer>().Kick(teammatePos, 200f * distance1);
            }
            else
            {
                context.navAgent.GetComponent<AgentSoccer>().Kick(teammatePos, 300f * distance1);
            }
        }
        return BTResult.SUCCESS;
    }
}
