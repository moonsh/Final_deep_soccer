using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class status_monitor : Action
{
    // The speed of the object
    public float speed = 0;
    private Transform[] red_agent;
    private Transform[] blue_agent;
    private Transform ball;

    public string red_tag = "redAgent";
    public string blue_tag = "blueAgent";
    public string ball_tag = "Ball";

    private soccer_ball test;
    public override void OnAwake()
    {
        // Cache all of the transforms that have a tag of targetTag
        var red_targets = GameObject.FindGameObjectsWithTag(red_tag);
        red_agent = new Transform[red_targets.Length];
        for (int i = 0; i < red_targets.Length; ++i)
        {
            red_agent[i] = red_targets[i].transform;
        }

        var blue_targets = GameObject.FindGameObjectsWithTag(blue_tag);
        blue_agent = new Transform[blue_targets.Length];
        for (int i = 0; i < blue_targets.Length; ++i)
        {
            blue_agent[i] = blue_targets[i].transform;
        }

        var ball_t = GameObject.FindGameObjectWithTag(ball_tag);
        ball = ball_t.transform;

        test = ball_t.GetComponent<soccer_ball>();

    }

    public override TaskStatus OnUpdate()
    {
        // Return a task status of success once we've reached the target
        if (true)
        {
            Debug.Log(Time.frameCount);
//            Debug.Log(test.owner);

            //for (int i = 0; i < blue_agent.Length; ++i)
            //{
            //    Debug.Log(blue_agent[i].transform.position.x);
            //    Debug.Log(blue_agent[i].transform.position.z);
            //}
            //for (int i = 0; i < red_agent.Length; ++i)
            //{
            //    Debug.Log(red_agent[i].transform.position.x);
            //    Debug.Log(red_agent[i].transform.position.z);
            //}



            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}