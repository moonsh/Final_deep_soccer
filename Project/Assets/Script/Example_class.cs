using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Example_class : MonoBehaviour
{

    public class soccer_info
    {
        public AgentSoccer Team_players;
        [HideInInspector]
        public Vector3 position;
        [HideInInspector]
        public Quaternion direction;
        [HideInInspector]
        public Rigidbody Rb;

        // Need to add ray intersection information (Check where is clear passing path )
        // Need to add which agent is free from enemy
        // Need to give good direction to move 
    }



    public SoccerEnvController envController;
    private Rigidbody agent_rigidbody;

    public int segments = 50;
    public float xradius = 2;
    public float yradius = 2;
    LineRenderer line;

    void CreatePoints()
    {
        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            line.SetPosition(i, new Vector3(x, 0, y));

            angle += (360f / segments);
        }
    }


    void Start()
    {
        envController = FindObjectOfType<SoccerEnvController>();
        agent_rigidbody = GetComponent<Rigidbody>();

        line = gameObject.GetComponent<LineRenderer>();

        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;
        CreatePoints();

    }

    //  m_ballcontrol.gameObject.GetComponent<Rigidbody>().AddForce(dir * 2000f);
    //  gameObject.GetComponent<Rigidbody>().drag = 6.0f;


    void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer

        foreach (var item in envController.AgentsList)
        {
            if (item.Rb.tag == agent_rigidbody.tag && item.Rb.position != agent_rigidbody.position)
            {
                Vector3 direction_to_agent;
                direction_to_agent = item.Rb.position - transform.position;
                direction_to_agent = direction_to_agent.normalized;



                if (Physics.Raycast(transform.position, direction_to_agent, out hit, Mathf.Infinity, layerMask))
                {
                    Debug.DrawRay(transform.position, direction_to_agent * hit.distance, Color.yellow);
                    if (hit.collider.tag == gameObject.tag)
                    {
                        //                print("You touched the TagName,  nice!");
                    }
                    else if(hit.collider.tag!=gameObject.tag)
                    {

                    }

                }
                else
                {
                    Debug.DrawRay(transform.position, direction_to_agent * 1000, Color.yellow);
                }
            }
        }
    }

}
