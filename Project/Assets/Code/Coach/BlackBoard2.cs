using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackBoard2 : MonoBehaviour
{
    
    public Toggle soccerPos;
    public Toggle teamPos;
    public Toggle oppoPos;
    
    public static bool soccerPosition;
    public static bool teamPosition;
    public static bool oppoPosition;
    
    public InputField soccerRange;
    public InputField teamRange;
    public InputField oppoRange;
    public static int agentR;
    public static int soccerR;
    public static int teamR;
    public static int oppoR;

    private void Start()
    {
        
        soccerPosition = true;
        teamPosition = true;
        oppoPosition = true;
        
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

        
        soccerR = int.Parse(soccerRange.text);
        teamR = int.Parse(teamRange.text);
        oppoR = int.Parse(oppoRange.text);
        CoachController.TeamR = teamR;
        CoachController.OppoR = oppoR;
        if( !teamPosition && !oppoPosition)
        {
            soccerPos.isOn = true;
        }
    }
}
