using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackBoard : MonoBehaviour
{
    public Text AgentLocation;
    public Text BallLocation;
    public Text TeammatesLocation;
    public Text TeammatesLocation2;
    public Text OpponentsLocation;
    public Text OpponentsLocation2;
    public Text time;
    public Transform SeletectedAgent;
    public Transform ball;
    public List<Transform> purpleTeam = new List<Transform>();
    public List<Transform> blueTeam = new List<Transform>();
    // Update is called once per frame
    void Update()
    {
        AgentLocation.text = SeletectedAgent.position.ToString();
        BallLocation.text = ball.position.ToString();
        string purpleTeamPos = "";
        string purpleTeamPos2 = "";
        string blueTeamPos = "";
        string blueTeamPos2 = "";
        int index = 0;
        foreach (Transform agent in purpleTeam)
        {
            if(agent != SeletectedAgent)
            {
                if (index < 2)
                    purpleTeamPos = purpleTeamPos + " " + agent.position.ToString();
                else
                    purpleTeamPos2 = purpleTeamPos2 + " " + agent.position.ToString();
                index++;
            }
        }
        index = 0;
        foreach (Transform agent in blueTeam)
        {
            if (agent != SeletectedAgent)
            {
                if (index < 2)
                    blueTeamPos = blueTeamPos + " " + agent.position.ToString();
                else
                    blueTeamPos2 = blueTeamPos2 + " " + agent.position.ToString();
                index++;
            }
        }
        if(SeletectedAgent.tag == "purpleAgent")
        {
            TeammatesLocation.text = purpleTeamPos;
            TeammatesLocation2.text = purpleTeamPos2;
            OpponentsLocation.text = blueTeamPos;
            OpponentsLocation2.text = blueTeamPos2;
        }
        else
        {
            TeammatesLocation.text = blueTeamPos;
            TeammatesLocation2.text = blueTeamPos2;
            OpponentsLocation.text = purpleTeamPos;
            OpponentsLocation2.text = purpleTeamPos2;
        }
        time.text = string.Format("{0:N3}", CoachController.countTime) + " seconds";
        
    }
}
