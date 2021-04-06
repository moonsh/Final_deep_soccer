// Anthony Tiongson (ast119)

public class BTUserActionsAssert : BTNode
{
    public override BTResult Execute()
    {
        if (context.userActions.Count == 0)
        {
            return BTResult.FAILURE;
        }
        else
        {
            return BTResult.SUCCESS;
        }
    }
}
