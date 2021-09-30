using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentSoccer2 : MonoBehaviour
{
    // Note that that the detectable tags are different for the blue and purple teams. The order is
    // * ball
    // * own goal
    // * opposing goal
    // * wall
    // * own teammate
    // * opposing player

    public enum Position
    {
        Striker,
        Goalie,
        Generic
    }

//    [HideInInspector]
    public enum Team
    {
        Red,
        Blue
    }



    float m_KickPower = 2000f;
    // The coefficient for the reward for colliding with a ball. Set using curriculum.
    float m_BallTouch;
    public Position position;
    public Team team;

    const float k_Power = 2000f;
    float m_Existential;
    float m_LateralSpeed;
    float m_ForwardSpeed;


//    [HideInInspector]
    public Rigidbody agentRb;
    public Vector3 initialPos;
    public float rotSign;
    private soccer_ball m_ballcontrol;
//    public Transform goal;

    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agentRb = GetComponent<Rigidbody>();
//        agent.destination = goal.position;
        initialPos = new Vector3(transform.position.x , .5f, transform.position.z );
        rotSign = 1f;

    }


    //void OnCollisionEnter(Collision c)
    //{
    //    var force = k_Power * m_KickPower;
    //    if (position == Position.Goalie)
    //    {
    //        force = k_Power;
    //    }

    //    if (c.gameObject.CompareTag("ball"))
    //    {
    //        var dir = c.contacts[0].point - transform.position;
    //        dir = dir.normalized;
    //        c.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
    //    }
    //}


}
