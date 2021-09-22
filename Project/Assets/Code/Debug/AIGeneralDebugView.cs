using UnityEngine;

public class AIGeneralDebugView : AIDebugView
{
    protected override string GetDebugText()
    {
        string viewText = "";

        AIComponent owningAgent = activeViewContext.owningContext.contextOwner;
        viewText += "Agent Name: " + owningAgent.name +"\n";
        viewText += "Tree Type: " + owningAgent.behaviourTreeType.ToString() + "\n";
        viewText += "AI State: " + owningAgent.currentState.ToString() + "\n";

        IEventSource currentTarget = owningAgent.currentTarget;

        if (currentTarget != null)
        {
            viewText += "Target: " + currentTarget.GetType().ToString() + "\n";
        }
        else viewText += "Target: " + "None" + "\n";

        Vector3 lkp = owningAgent.sensorySystem.lastKnownPosition;
        if ( lkp != Vector3.zero)
        {
            viewText += "Last Known Position: " + lkp.ToString();
        }

        switch (owningAgent.currentState)
        {
            case AIState.IDLE:
                debugStyle.normal.textColor = Color.green;
                break;
            case AIState.ALERT:
                debugStyle.normal.textColor = Color.yellow;
                break;
            case AIState.HOSTILE:
                debugStyle.normal.textColor = Color.red;
                break;
            default:
                break;
        }

        return viewText;
    }
}
