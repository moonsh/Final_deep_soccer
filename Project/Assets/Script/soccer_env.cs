using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class soccer_env : MonoBehaviour
{
    [System.Serializable]
    public class PlayerInfo
    {
        public AgentSoccer Agent;
        [HideInInspector]
        public Vector3 StartingPos;
        [HideInInspector]
        public Quaternion StartingRot;
        [HideInInspector]
        public Rigidbody Rb;
    }

    /// <summary>
    /// Max Academy steps before this platform resets
    /// </summary>
    /// <returns></returns>
    [Header("Max Environment Steps")] public int MaxEnvironmentSteps = 25000;

    /// <summary>
    /// The area bounds.
    /// </summary>

    /// <summary>
    /// We will be changing the ground material based on success/failue
    /// </summary>

    public GameObject ball;
    [HideInInspector]
    public Rigidbody ballRb;
    Vector3 m_BallStartingPos;

    //List of Agents On Platform
    public List<PlayerInfo> AgentsList = new List<PlayerInfo>();

    [Range(0, 100)]
    public int stealProbability = 50;
    public Text purpleScore;
    public Text blueScore;


    private int m_ResetTimer;
    private Vector3 current_position;
    private float distance_cal;

    private Vector3 calculate_distance_ball_agents;
    private float WaitTime = 3.0f;
    private float Timer = 0.0f;

    void Start()
    {

        ballRb = ball.GetComponent<Rigidbody>();
        m_BallStartingPos = new Vector3(ball.transform.position.x, ball.transform.position.y, ball.transform.position.z);

        foreach (var item in AgentsList)
        {
            item.StartingPos = item.Agent.transform.position;
            item.StartingRot = item.Agent.transform.rotation;
            item.Rb = item.Agent.GetComponent<Rigidbody>();
        }


        ResetScene();
    }

    void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            ResetScene();
        }

        foreach (var item in AgentsList)
        {
            current_position = item.Agent.transform.position;

            calculate_distance_ball_agents = current_position - ball.transform.position;
            calculate_distance_ball_agents.y = 0;
            distance_cal = calculate_distance_ball_agents.magnitude;

            if (distance_cal < 1.25)
            {
                if (ball.GetComponent<soccer_ball>().owner == null)
                {
                    ball.GetComponent<soccer_ball>().owner = item.Agent.gameObject;
                }
                else
                {
                    if (ball.GetComponent<soccer_ball>().owner.tag != item.Agent.gameObject.tag & Timer > WaitTime)
                    {
                        Timer = 0f;
                        int temp = UnityEngine.Random.Range(0, 100);
                        if (temp < stealProbability)
                        {
                            ball.GetComponent<soccer_ball>().owner = item.Agent.gameObject;
                        }
                    }
                }
            }
        }

        Timer += Time.deltaTime;

        if (Mathf.Abs(ball.transform.position.z) > 50f || Mathf.Abs(ball.transform.position.x) > 30.5f)
        {
            ResetScene();
        }
    }

    public void ResetBall()
    {
        ball.GetComponent<soccer_ball>().owner = null;
        ball.transform.position = m_BallStartingPos;
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
    }

    public void GoalTouched(int i)
    {
        if (i == 1)
        {
//            purpleScore.text = (Int16.Parse(purpleScore.text) + 1).ToString();
            //Debug.Log("SoccerEnvController: purple team has scored, resetting scene.");
            //Debug.Log("SoccerEnvController: CoachController.actionSequence.Count = " + CoachController.actionSequence.Count);

            //if (CoachController.actionSequence.Count > 0)
            //{
            //    List<string> actions = new List<string>(CoachController.actionSequence);
            //    CoachController.actionSequences.Add(actions);
            //    //Debug.Log("SoccerEnvController: CoachController.actionSequences.Count = " + CoachController.actionSequences.Count);
            //    //Debug.Log("SoccerEnvController: recording successful action sequences.");
            //}
        }
        else
        {
  //          blueScore.text = (Int16.Parse(blueScore.text) + 1).ToString();
        }

        ResetScene();
        //Debug.Log("SoccerEnvController: CoachController.actionSequences[0].Count = " + CoachController.actionSequences[0].Count);
    }


    public void ResetScene()
    {
        m_ResetTimer = 0;

        //Reset Agents
        foreach (var item in AgentsList)
        {
            var newStartPos = item.Agent.initialPos;
            var rot = item.Agent.rotSign;
            var newRot = Quaternion.Euler(0, rot, 0);
            item.Agent.transform.SetPositionAndRotation(item.StartingPos, newRot);
            item.Rb.velocity = Vector3.zero;
            item.Rb.angularVelocity = Vector3.zero;
            item.Agent.GetComponent<NavMeshAgent>().SetDestination(item.Agent.transform.position);
        }

//        CoachController.sceneReset = true;
//        CoachController.actionSequence.Clear();
        //Reset Ball
        ResetBall();
    }
}
