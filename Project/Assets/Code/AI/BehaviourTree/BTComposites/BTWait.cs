using System.Collections.Generic;
using UnityEngine;
using XNode;

[BTComposite(typeof(BTWait))]
public class BTWait : BTNode
{
    public enum WaitType 
    {
        EXECUTE_AND_WAIT,
        WAIT_AND_EXECUTE
    }

    class WaitData
    {
        public WaitData(float _waitTime, BTContext _waitingContext)
        {
            waitTime = _waitTime;
            waitingContext = _waitingContext;
        }

        public float waitTime;
        public BTContext waitingContext;
    }

    [Input] public BTResult inResult;
    [Input] public BTResult interruptIfTrue;

    public WaitType waitType;
    public float waitTime = 1;

    List<WaitData> waitDataList = new List<WaitData>();
    BTResult waitingNodeResult = BTResult.SUCCESS;

    BehaviourTreeManager behaviourTreeManager;

    public override void OnStart()
    {
        behaviourTreeManager = BehaviourTreeManager.GetInstance();
    }

    public override BTResult Execute()
    {
        NodePort inPort = GetPort("inResult");
        if (inPort != null && inPort.GetConnections().Count != 0)
        {
            NodePort connection = inPort.GetConnection(0);

            if (connection != null)
            {
                BTResult result = BTResult.XRUNNING_DO_NOT_USE;

                WaitData activeWaitData = waitDataList.Find(x => x.waitingContext == context);
                if (activeWaitData == null)
                {
                    if (waitType == WaitType.EXECUTE_AND_WAIT)
                    {
                        waitingNodeResult = (BTResult)connection.GetOutputValue();
                    }

                    float initialWaitTime = connection.node is IBTWaitingNode ? ((IBTWaitingNode)connection.node).GetWaitTime() : waitTime;

                    if (initialWaitTime == 0 || WaitInterrupted()) 
                    {
                        //If wait time is 0, ignore running node and exit
                        return BTResult.FAILURE;
                    }

                    activeWaitData = new WaitData(initialWaitTime, context);
                    waitDataList.Add(activeWaitData);

                    BehaviourTreeRuntimeData.AddRunningNode(context, this);

                    result = BTResult.XRUNNING_DO_NOT_USE;
                }

                float btUpdateRate = behaviourTreeManager.updateRate;
                activeWaitData.waitTime -= Time.deltaTime + btUpdateRate;

                if (activeWaitData.waitTime <= 0 || WaitInterrupted())
                {
                    if (waitType == WaitType.WAIT_AND_EXECUTE) 
                    {
                        waitingNodeResult = (BTResult)connection.GetOutputValue();
                    }

                    BehaviourTreeRuntimeData.RemoveRunningNode(context, this);
                    waitDataList.Remove(activeWaitData);
                    return waitingNodeResult;
                }
                else result = BTResult.XRUNNING_DO_NOT_USE;

                return result;
            }
        }
        return BTResult.SUCCESS;
    }

    bool WaitInterrupted()
    {
        bool result = false;
        NodePort inPort = GetPort("interruptIfTrue");
        if (inPort != null && inPort.GetConnections().Count != 0)
        {
            NodePort connection = inPort.GetConnection(0);

            if (connection != null)
            {
                result = (BTResult)connection.GetOutputValue() == BTResult.SUCCESS;
            }
        }

        return result;
    }
}

public interface IBTWaitingNode 
{
    float GetWaitTime();
}
