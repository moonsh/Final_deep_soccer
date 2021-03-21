// Anthony Tiongson (ast119)
// Robo-Soccer

using System;
using System.Collections.Generic;
using System.Xml;
using System.Threading.Tasks;
using System.Linq;
using XNode;

public class BTSequenceParallel : BTNode
{
    /* Advice:
     * If you dont need unity apis, and want parallel, its pretty easy to do... 
     * Basically you copy the sequence code, but for each connected node
     * do a Task.Run(Action<T>);
     * task returns a Task and you can place it in an array Tasks[] tasks
     * and then to Task.WaitForAll(tasks) in the Parallel node,
     * or check them individually
     * Resources:
     * https://john-tucker.medium.com/unity-leveling-up-with-async-await-tasks-2a7971df9c57
     * https://stackoverflow.com/questions/13257458/check-if-a-value-is-in-an-array-c
     * https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.waitall?view=net-5.0
     */

    [Input] public List<BTResult> inResults;

    public override BTResult Execute()
    {
        NodePort inPort = GetPort("inResults");

        if (inPort != null)
        {
            List<NodePort> connections = inPort.GetConnections();
            int arraySize = connections.Capacity;
            Task[] tasks = new Task[arraySize - 1];
            BTResult[] results = new BTResult[arraySize - 1];
            int index = 0;

            foreach (NodePort _port in connections)
            {
                /*
                BTResult result = (BTResult)_port.GetOutputValue();
                if (result == BTResult.FAILURE) { return BTResult.FAILURE; }
                if (result == BTResult.XRUNNING_DO_NOT_USE) { return BTResult.XRUNNING_DO_NOT_USE; }
                */

                tasks[index] = Task.Run(() => results[index] = (BTResult)_port.GetOutputValue());
                index++;
            }

            //return BTResult.SUCCESS;

            try
            {
                // Wait for all the tasks to finish.
                Task.WaitAll(tasks);

                // Continue with main thread
                if (results.Contains(BTResult.FAILURE))
                {
                    return BTResult.FAILURE;
                }
                else
                {
                    return BTResult.SUCCESS;
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
 