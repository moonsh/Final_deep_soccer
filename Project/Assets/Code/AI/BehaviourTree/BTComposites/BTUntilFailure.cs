using XNode;

[BTComposite(typeof(BTUntilFailure))]
public class BTUntilFailure : BTNode
{
    [Input] public BTResult inResult;
    [Input] public BTResult onNodeExit;

    BehaviourTreeManager behaviourTreeManager;

    public override void OnStart()
    {
        behaviourTreeManager = BehaviourTreeManager.GetInstance();
    }

    public override BTResult Execute()
    {
        BTResult result = BTResult.SUCCESS;
        NodePort inPort = GetPort("inResult");
        if (inPort != null && inPort.GetConnections().Count != 0)
        {
            NodePort connection = inPort.GetConnection(0);

            if (connection != null)
            {
                result = (BTResult)connection.GetOutputValue();
                if (result == BTResult.FAILURE)
                {
                    BehaviourTreeRuntimeData.RemoveRunningNode(context, this);
                    ExecuteExitNode();
                    result = BTResult.SUCCESS;
                }
                else if (result == BTResult.XRUNNING_DO_NOT_USE)
                {
                    result = BTResult.XRUNNING_DO_NOT_USE;
                }
                else
                {
                    BehaviourTreeRuntimeData.AddRunningNode(context, this);
                    result = BTResult.XRUNNING_DO_NOT_USE;
                }
            }
        }
        return result;
    }

    void ExecuteExitNode()
    {
        NodePort inPort = GetPort("onNodeExit");
        if (inPort != null && inPort.GetConnections().Count != 0)
        {
            NodePort connection = inPort.GetConnection(0);

            if (connection != null)
            {
                connection.GetOutputValue();
            }
        }
    }
}
