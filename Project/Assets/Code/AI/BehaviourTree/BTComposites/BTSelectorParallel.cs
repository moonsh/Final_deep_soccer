// Anthony Tiongson (ast119)
// Robo-Soccer

using System;
using System.Collections.Generic;
using System.Xml;
using System.Threading.Tasks;
using System.Linq;
using XNode;

public class BTSelectorParallel : BTNode
{
    // Derived from BTSequenceParallel and BTSelector

    [Input] public List<BTResult> inResults;

    public override BTResult Execute()
    {
        NodePort inPort = GetPort("inResults");

        if (inPort != null)
        {
            List<NodePort> connections = inPort.GetConnections();
            int arraySize = connections.Capacity;
            Task[] tasks = new Task[arraySize];
            BTResult[] results = new BTResult[arraySize];
            int index = 0;

            foreach (NodePort _port in connections)
            {
                /*
                BTResult result = (BTResult)_port.GetOutputValue();
                if (result == BTResult.SUCCESS) { return BTResult.SUCCESS; }
                if (result == BTResult.XRUNNING_DO_NOT_USE) { return BTResult.XRUNNING_DO_NOT_USE; }
                */

                tasks[index] = Task.Run(() => results[index] = (BTResult)_port.GetOutputValue());
                index++;
            }

            //return BTResult.FAILURE;

            try
            {
                // Wait for all the tasks to finish.
                Task.WaitAll(tasks);

                // Continue with main thread
                if (results.Contains(BTResult.SUCCESS))
                {
                    return BTResult.SUCCESS;
                }
                else
                {
                    return BTResult.FAILURE;
                }
            }
            catch (AggregateException e)
            {
                Console.WriteLine("\nThe following exceptions have been thrown by WaitAll(): ");

                for (int i = 0; i < e.InnerExceptions.Count; i++)
                {
                    Console.WriteLine("\n-------------------------------------------------\n{0}", e.InnerExceptions[i].ToString());
                }

                return BTResult.FAILURE;
            }
        }
        else return BTResult.FAILURE;
    }
}