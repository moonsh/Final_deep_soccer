using System.Collections.Generic;

public static class BehaviourTreeRuntimeData
{
    static Dictionary<BehaviourTreeType, RuntimeBehaviourTree> behaviourTreeMap = new Dictionary<BehaviourTreeType, RuntimeBehaviourTree>();
    static Dictionary<BehaviourTreeType, List<BTContextData>> contextMap = new Dictionary<BehaviourTreeType, List<BTContextData>>();

    
    public static void RegisterBehaviourTree(RuntimeBehaviourTree _behaviourTree)
    {
        behaviourTreeMap[_behaviourTree.behaviourTreeType] = _behaviourTree;
    }

    public static void RegisterAgentContext(BehaviourTreeType _behaviourTreeType, BTContext _aiContext)
    {
        if (!contextMap.ContainsKey(_behaviourTreeType))
        {
            contextMap[_behaviourTreeType] = new List<BTContextData>();
        }

        contextMap[_behaviourTreeType].Add(new BTContextData(_aiContext));
    }

    public static void UnregisterAgentContext(BehaviourTreeType _behaviourTreeType, BTContext _aiContext) 
    {
        if (contextMap.ContainsKey(_behaviourTreeType)) 
        {
            BTContextData data = contextMap[_behaviourTreeType].Find(x => x.owningContext == _aiContext);
            if (data != null) 
            {
                contextMap[_behaviourTreeType].Remove(data);
            }
        }
    }

    public static void AddRunningNode(BTContext _context, BTNode _node)
    {
        BTContextData data = contextMap[_context.contextOwner.behaviourTreeType].Find(x => x.owningContext == _context);
        if (data != null)
        {
            data.AddRunningNode(_node);
        }
    }

    public static void RemoveRunningNode(BTContext _context, BTNode _node)
    {
        BTContextData data = contextMap[_context.contextOwner.behaviourTreeType].Find(x => x.owningContext == _context);
        if (data != null)
        {
            data.RemoveRunningNode(_node);
        }
    }

    public static Dictionary<BehaviourTreeType, RuntimeBehaviourTree> GetBehaviourTrees()
    {
        return behaviourTreeMap;
    }

    public static Dictionary<BehaviourTreeType, List<BTContextData>> GetContextData()
    {
        return contextMap;
    }
}
