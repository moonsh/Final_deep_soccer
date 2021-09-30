using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Hard_coded_AI : Action
{
    // The speed of the object

    public override void OnAwake()
    {

    }

    public override TaskStatus OnUpdate()
    {
        // Return a task status of success once we've reached the target
        if (true)
        {

            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}