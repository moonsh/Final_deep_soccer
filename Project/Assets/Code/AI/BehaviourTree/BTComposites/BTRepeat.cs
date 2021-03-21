using System.Collections.Generic;
using UnityEngine;
using XNode;

[BTComposite(typeof(BTRepeat))]
public class BTRepeat : BTNode
{
    class RepeatData
    {
        public RepeatData(int _repeatCount, BTContext _repeatingContext)
        {
            repeatCount = _repeatCount;
            repeatingContext = _repeatingContext;
        }

        public int repeatCount;
        public BTContext repeatingContext;
    }

    [Input] public BTResult inResult;
    public int repeatCount = 2;

    List<RepeatData> repeatDataList = new List<RepeatData>();

    public override BTResult Execute()
    {
        NodePort inPort = GetPort("inResult");
        if (inPort != null && inPort.GetConnections().Count != 0)
        {
            NodePort connection = inPort.GetConnection(0);

            if (connection != null)
            {
                BTResult result = (BTResult)connection.GetOutputValue();

                RepeatData activeRepeatData = repeatDataList.Find(x => x.repeatingContext == context);
                if (activeRepeatData == null)
                {
                    activeRepeatData = new RepeatData(repeatCount, context);
                    repeatDataList.Add(activeRepeatData);

                    BehaviourTreeRuntimeData.AddRunningNode(context, this);
                    result = BTResult.XRUNNING_DO_NOT_USE;
                }

                --activeRepeatData.repeatCount;

                if (activeRepeatData.repeatCount <= 0)
                {
                    BehaviourTreeRuntimeData.RemoveRunningNode(context, this);
                    repeatDataList.Remove(activeRepeatData);
                }
                else result = BTResult.XRUNNING_DO_NOT_USE;

                return result;
            }
        }
        return BTResult.SUCCESS;
    }
}

