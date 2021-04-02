using UnityEngine;

public class SoccerBallController : MonoBehaviour
{
    public GameObject area;
    public GameObject owner;
    [HideInInspector]
    public SoccerEnvController envController;
    public string purpleGoalTag; //will be used to check if collided with purple goal
    public string blueGoalTag; //will be used to check if collided with blue goal

    void Start()
    {
        envController = area.GetComponent<SoccerEnvController>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(purpleGoalTag)) //ball touched purple goal
        {
            envController.GoalTouched(Team.Blue);
        }
        if (col.gameObject.CompareTag(blueGoalTag)) //ball touched blue goal
        {
            envController.GoalTouched(Team.Purple);
        }
        if(col.gameObject.tag == "wall")
        {
            envController.ResetBall();
        }
    }


    void Update()
    {
        if (owner)
        {
            //Debug.Log(owner.transform.forward);
            //            transform.position = owner.transform.position + owner.transform.forward * 1.5f + owner.transform.up  * 1.0f;
            transform.position = owner.transform.position + owner.transform.forward * 1.55f + owner.transform.up;
            //float velocity = owner.GetComponent<AgentSoccer>().v.magnitude; //rapidité
                                                                                                 // Debug.Log(velocity);
            transform.Rotate(owner.transform.right, 0.1f * 10f);

        }
    }


}

