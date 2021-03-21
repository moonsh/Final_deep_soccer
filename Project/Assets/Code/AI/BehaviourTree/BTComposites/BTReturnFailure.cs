[BTComposite(typeof(BTReturnFailure))]
public class BTReturnFailure : BTNode
{
    [Input] public BTResult inResult;

    public override BTResult Execute()
    {
        BTResult result = GetInputValue("inResult", BTResult.SUCCESS);
        if (result == BTResult.XRUNNING_DO_NOT_USE)
        {
            return BTResult.XRUNNING_DO_NOT_USE;
        }
        else return BTResult.FAILURE;
    }
}
