using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackBoard2 : MonoBehaviour
{
    public Toggle agentPos;
    public Toggle soccerPos;
    public Toggle teamPos;
    public Toggle oppoPos;
    public static bool agentPosition;
    public static bool soccerPosition;
    public static bool teamPosition;
    public static bool oppoPosition;
    public InputField agentRange;
    public InputField soccerRange;
    public InputField teamRange;
    public InputField oppoRange;
    public static int agentR;
    public static int soccerR;
    public static int teamR;
    public static int oppoR;

    private void Start()
    {
        agentPosition = true;
        soccerPosition = true;
        teamPosition = true;
        oppoPosition = true;
        agentRange.text = "5";
        soccerRange.text = "5";
        teamRange.text = "5";
        oppoRange.text = "5";
        agentR = 5;
        soccerR = 5;
        teamR = 5;
        oppoR = 5;
    }

    void Update()
    {
        if(agentPos.isOn)
            agentPosition = true;
        else
            agentPosition = false;
        if(soccerPos.isOn)
            soccerPosition = true;
        else
            soccerPosition = false;
        if(teamPos.isOn)
            teamPosition = true;
        else
            teamPosition = false;
        if (oppoPos.isOn)
            oppoPosition = true;
        else
            oppoPosition = false;

        agentR = int.Parse(agentRange.text);
        soccerR = int.Parse(soccerRange.text);
        teamR = int.Parse(teamRange.text);
        oppoR = int.Parse(oppoRange.text);
        if(!agentPosition&& !soccerPosition&& !teamPosition && !oppoPosition)
        {
            agentPos.isOn = true;
        }
    }
}
