using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackBoard : MonoBehaviour
{
    public Text Title;
    public Text AgentLocation;
    public Text BallLocation;
    public Text TeammatesLocation;
    public Text TeammatesLocation2;
    public Text OpponentsLocation;
    public Text OpponentsLocation2;
    public Text OpponentsLocation3;
    public Text BallPossessor;
    public Text DistanceToGoal;
    public Text DirectionToGoal;
    public Text NearByOpponents;
    public Text PassableTeammates;
    public Text time;
    public Text BlueScore;
    public Text PurpleScore;
    public Text BlueScore1;
    public Text PurpleScore1;
    public Transform SeletectedAgent;
    public Transform ball;
    public Transform PurpleGoal;
    public Transform BlueGoal;
    public List<Transform> purpleTeam = new List<Transform>();
    public List<Transform> blueTeam = new List<Transform>(); 
    // Update is called once per frame
    void Update()
    {
        Title.text = SeletectedAgent.name.Substring(SeletectedAgent.name.Length - 2) + " Board";
        AgentLocation.text = SeletectedAgent.position.ToString();
        BallLocation.text = ball.position.ToString();
        BlueScore.text = BlueScore1.text;
        PurpleScore.text = PurpleScore1.text;

        if (ball.GetComponent<SoccerBallController>().owner != null)
        {
            string ownername = ball.GetComponent<SoccerBallController>().owner.name;
            BallPossessor.text = ownername.Substring(ownername.Length - 2);
        }
        else
        {
           if(ball.position.z<0)
           {
                BallPossessor.text = "Purple Side";
           }
           else if (ball.position.z == 0)
           {
                BallPossessor.text = "Center";
           }
           else
           {
                BallPossessor.text = "Blue Side";
           }
        }
        string purpleTeamPos = "";
        string purpleTeamPos2 = "";
        string purpleTeamPos3 = "";
        string blueTeamPos = "";
        string blueTeamPos2 = "";
        string blueTeamPos3 = "";
        int index = 0;
        foreach (Transform agent in purpleTeam)
        {
            if(agent != SeletectedAgent)
            {
                if (index < 1)
                    purpleTeamPos = purpleTeamPos + " " + agent.name.Substring(agent.name.Length - 2) +":"+ agent.position.ToString();
                else if (index < 2)
                    purpleTeamPos2 = purpleTeamPos2 + " " + agent.name.Substring(agent.name.Length - 2) + ":" + agent.position.ToString();
                else
                    purpleTeamPos3 = purpleTeamPos3 + " " + agent.name.Substring(agent.name.Length - 2) + ":" + agent.position.ToString();
                index++;
            }
        }
        index = 0;
        foreach (Transform agent in blueTeam)
        {
            if (agent != SeletectedAgent)
            {
                if (index < 1)
                    blueTeamPos = blueTeamPos + " " + agent.name.Substring(agent.name.Length - 2) + ":" + agent.position.ToString();
                else if (index < 2)
                    blueTeamPos2 = blueTeamPos2 + " " + agent.name.Substring(agent.name.Length - 2) + ":"  + agent.position.ToString();
                else
                    blueTeamPos3 = blueTeamPos3 + " " + agent.name.Substring(agent.name.Length - 2) + ":" + agent.position.ToString();
                index++;
            }
        }
        if(SeletectedAgent.tag == "purpleAgent")
        {
            TeammatesLocation.text = purpleTeamPos;
            TeammatesLocation2.text = purpleTeamPos2;
            OpponentsLocation.text = blueTeamPos;
            OpponentsLocation2.text = blueTeamPos2;
            OpponentsLocation3.text = blueTeamPos3;
            float distance = Mathf.Sqrt((SeletectedAgent.position.x - PurpleGoal.position.x) * (SeletectedAgent.position.x - PurpleGoal.position.x) + (SeletectedAgent.position.z - PurpleGoal.position.z) * (SeletectedAgent.position.z - PurpleGoal.position.z));
            DistanceToGoal.text = string.Format("{0:N3}", distance);
            Vector3 direction = (PurpleGoal.position - SeletectedAgent.position).normalized;
            DirectionToGoal.text = direction.ToString();
            int count = 0;
            foreach (Transform agent in blueTeam)
            {
                float distanceToOpponent = Mathf.Sqrt((SeletectedAgent.position.x - agent.position.x) * (SeletectedAgent.position.x - agent.position.x) + (SeletectedAgent.position.z - agent.position.z) * (SeletectedAgent.position.z - agent.position.z));
                if(distanceToOpponent < 7)
                {
                    count++;
                    if(count==1)
                    {
                        NearByOpponents.text = agent.name.Substring(agent.name.Length - 2);
                    }
                    else
                    {
                        NearByOpponents.text = NearByOpponents.text + "," + agent.name.Substring(agent.name.Length - 2);
                    }
                }
            }
            if(count==0)
            {
                NearByOpponents.text = "N/A";
            }
            count = 0;
            RaycastHit hit;
            foreach (Transform agent in purpleTeam)
            {
                Vector3 direction_to_agent;
                direction_to_agent = agent.transform.position - SeletectedAgent.transform.position;
                direction_to_agent = direction_to_agent.normalized;
                if (Physics.Raycast(SeletectedAgent.transform.position, direction_to_agent, out hit, Mathf.Infinity))
                {
                    if (hit.collider.name == agent.name)
                    {
                        count++;
                        if (count == 1)
                        {
                            PassableTeammates.text = agent.name.Substring(agent.name.Length - 2);
                        }
                        else
                        {
                            PassableTeammates.text = PassableTeammates.text + "," + agent.name.Substring(agent.name.Length - 2);
                        }
                    }

                }
            }
            if (count == 0)
            {
                PassableTeammates.text = "N/A";
            }
        }
        else
        {
            TeammatesLocation.text = blueTeamPos;
            TeammatesLocation2.text = blueTeamPos2;
            OpponentsLocation.text = purpleTeamPos;
            OpponentsLocation2.text = purpleTeamPos2;
            OpponentsLocation3.text = purpleTeamPos3;
            float distance = Mathf.Sqrt((SeletectedAgent.position.x - BlueGoal.position.x) * (SeletectedAgent.position.x - BlueGoal.position.x) + (SeletectedAgent.position.z - BlueGoal.position.z) * (SeletectedAgent.position.z - BlueGoal.position.z));
            DistanceToGoal.text = string.Format("{0:N3}", distance);
            Vector3 direction = (BlueGoal.position - SeletectedAgent.position).normalized;
            DirectionToGoal.text = direction.ToString();
            int count = 0;
            foreach (Transform agent in purpleTeam)
            {
                float distanceToOpponent = Mathf.Sqrt((SeletectedAgent.position.x - agent.position.x) * (SeletectedAgent.position.x - agent.position.x) + (SeletectedAgent.position.z - agent.position.z) * (SeletectedAgent.position.z - agent.position.z));
                if (distanceToOpponent < 7)
                {
                    count++;
                    if (count == 1)
                    {
                        NearByOpponents.text = agent.name.Substring(agent.name.Length - 2);
                    }
                    else
                    {
                        NearByOpponents.text = NearByOpponents.text + "," + agent.name.Substring(agent.name.Length - 2);
                    }
                }
            }
            if (count == 0)
            {
                NearByOpponents.text = "N/A";
            }
            count = 0;
            RaycastHit hit;
            foreach (Transform agent in blueTeam)
            {
                Vector3 direction_to_agent;
                direction_to_agent = agent.transform.position - SeletectedAgent.transform.position;
                direction_to_agent = direction_to_agent.normalized;
                if (Physics.Raycast(SeletectedAgent.transform.position, direction_to_agent, out hit, Mathf.Infinity))
                {
                    if (hit.collider.name == agent.name)
                    {
                        count++;
                        if(count==1)
                        {
                            PassableTeammates.text = agent.name.Substring(agent.name.Length - 2);
                        }
                        else
                        {
                            PassableTeammates.text = PassableTeammates.text + "," + agent.name.Substring(agent.name.Length - 2);
                        }
                    }

                }
            }
            if (count == 0)
            {
                PassableTeammates.text = "N/A";
            }
        }
        time.text = string.Format("{0:N3}", CoachController.countTime) + " seconds";
        
    }
}
