using UnityEngine;

public class AIBehaviourDebugView : AIDebugView
{
    protected override string GetDebugText()
    {
        string viewText = "";
#if UNITY_EDITOR
        debugStyle.normal.textColor = Color.green;
        activeViewContext.owningContext.behaviourHistory.ForEach(x => viewText += x + "\n");
#endif // UNITY_EDITOR
        return viewText;
    }
}
