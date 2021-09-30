using UnityEngine;

public class soccer_ball : MonoBehaviour
{
    public GameObject area;
    public GameObject owner;
//    [HideInInspector]
    public soccer_env envController;
    public string redGoalTag= "redGoal"; //will be used to check if collided with purple goal
    public string blueGoalTag = "blueGoal"; //will be used to check if collided with blue goal

    void Start()
    {
        envController = area.GetComponent<soccer_env>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(redGoalTag)) //ball touched purple goal
        {
            envController.GoalTouched(1);
        }
        if (col.gameObject.CompareTag(blueGoalTag)) //ball touched blue goal
        {
            envController.GoalTouched(0);
        }
        if (col.gameObject.tag == "wall")
        {
            envController.ResetScene();
        }
    }


    void Update()
    {
        if (owner)
        {
            //Debug.Log(owner.transform.forward);
            //            transform.position = owner.transform.position + owner.transform.forward * 1.5f + owner.transform.up  * 1.0f;
            Vector3 test = owner.transform.position + owner.transform.forward * 1.55f + owner.transform.up;
            test.y = 0.25f;
            transform.position =test;
            //float velocity = owner.GetComponent<AgentSoccer>().v.magnitude; //rapidité
            // Debug.Log(velocity);
            //transform.Rotate(owner.transform.right, 0.1f * 10f);

            if (owner.tag == "blueAgent")
            {
                envController.ResetScene();
            }
        }
    }
}

