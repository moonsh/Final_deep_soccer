using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AgentScenarioIndicatorController : MonoBehaviour
{
    [SerializeField] private GameObject agentScenarioIndicator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var agent in CoachController.agentsUsingPastScenario)
        {
            Debug.Log("Agent: " + agent.ToString() + "\n" +
                "Scenario: " + agent.pastScenarios[0].ToString());
        }
    }
}
