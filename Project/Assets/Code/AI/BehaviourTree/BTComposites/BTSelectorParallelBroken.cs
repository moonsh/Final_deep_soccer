// Anthony Tiongson (ast119)
// Robo-Soccer

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XNode;
using UnityEngine;

[BTComposite(typeof(BTSelectorParallelBroken))]
public class BTSelectorParallelBroken : BTNode
{
    // Derived from BTSequenceParallel and BTSelector
    // Broken

    [Input] public List<BTResult> inResults;

    public override BTResult Execute()
    {
        NodePort inPort = GetPort("inResults");

        if (inPort != null)
        {
            List<NodePort> connections = inPort.GetConnections();
            var tasks = new List<Task>();
            var results = new List<BTResult>();

            foreach (NodePort _port in connections)
            {
                /*
                BTResult result = (BTResult)_port.GetOutputValue();
                if (result == BTResult.SUCCESS) { return BTResult.SUCCESS; }
                if (result == BTResult.XRUNNING_DO_NOT_USE) { return BTResult.XRUNNING_DO_NOT_USE; }
                */

                tasks.Add(Task.Run(() => {

                    results.Add((BTResult)_port.GetOutputValue());
                }));
            }

            //return BTResult.FAILURE;

            try
            {
                // Wait for all the tasks to finish.
                Task.WaitAll(tasks.ToArray());

                // Continue with main thread
                if (results.Contains(BTResult.XRUNNING_DO_NOT_USE))
                {
                    Debug.Log("BTResult: XRUNNING_DO_NOT_USE");
                    return BTResult.XRUNNING_DO_NOT_USE;
                }
                else
                {
                    if (results.Contains(BTResult.SUCCESS))
                    {
                        return BTResult.SUCCESS;
                    }
                    else
                    {
                        return BTResult.FAILURE;
                    }
                }
            }
            catch (AggregateException e)
            {
                Debug.Log("\nThe following exceptions have been thrown by WaitAll(): ");

                for (int i = 0; i < e.InnerExceptions.Count; i++)
                {
                    Debug.Log("\n-------------------------------------------------\n{0}" + e.InnerExceptions[i].ToString());
                }

                return BTResult.FAILURE;
            }
        }
        else return BTResult.FAILURE;
    }
}
