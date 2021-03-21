using System;
using System.Collections.Generic;
using XNode;
using UnityEngine;

public class BehaviourTreeManager : Singleton<BehaviourTreeManager>
{
    Dictionary<BehaviourTreeType, RuntimeBehaviourTree> behaviourTreeMap;
    Dictionary<BehaviourTreeType, List<BTContextData>> contextMap;

    bool behaviourTreeStarting = true;

    public float updateRate = 0.5f;
    public float updateTimer = 0;

    private void Start()
    {
        behaviourTreeMap = BehaviourTreeRuntimeData.GetBehaviourTrees();
        contextMap = BehaviourTreeRuntimeData.GetContextData();
    }

    private void Update()
    {
        if (behaviourTreeStarting)
        {
            behaviourTreeStarting = false;
            InitializeAllBehaviourTrees(); 
        }

        if (updateTimer > updateRate)
        {
#if UNITY_EDITOR
            ClearAgentHistory();
#endif //UNITY_EDITOR
            updateTimer = 0;
            RunAllAgents();
        }
        else updateTimer += Time.deltaTime;
    }

    private void RunAllAgents()
    {
        for (int i = 0; i < (int)BehaviourTreeType.COUNT; ++i)
        {
            BehaviourTreeType treeType = (BehaviourTreeType)i;

            if (behaviourTreeMap.ContainsKey(treeType))
            {
                if (contextMap.ContainsKey(treeType))
                {
                    contextMap[treeType].ForEach(x => behaviourTreeMap[treeType].RunBehaviourTree(x));
                }
            }
        }
    }

    void InitializeAllBehaviourTrees()
    {
        for (int i = 0; i < (int)BehaviourTreeType.COUNT; ++i)
        {
            BehaviourTreeType treeType = (BehaviourTreeType)i;

            if (behaviourTreeMap.ContainsKey(treeType))
            {
                InitializeTreeNodes(behaviourTreeMap[treeType].runtimeTree.nodes); 
            }
        }
    }

    void InitializeTreeNodes(List<Node> _nodeList) 
    {
        foreach (Node _node in _nodeList)
        {
            BTNode btNode = (BTNode)_node;
            if (btNode != null)
            {
                if (btNode is BTSubTree)
                {
                    btNode.OnStart();
                    InitializeTreeNodes(((BTSubTree)btNode).subTree.nodes);
                }
                else btNode.OnStart();
            }
        }
    }

    public List<BTContextData> GetAllContextData()
    {
        List<BTContextData> dataList = new List<BTContextData>();

        foreach (KeyValuePair<BehaviourTreeType, List<BTContextData>> _kvp in contextMap)
        {
            _kvp.Value.ForEach(x => dataList.Add(x));
        }

        return dataList;
    }

#if UNITY_EDITOR
    private void ClearAgentHistory()
    {
        for (int i = 0; i < (int)BehaviourTreeType.COUNT; ++i)
        {
            BehaviourTreeType treeType = (BehaviourTreeType)i;

            if (behaviourTreeMap.ContainsKey(treeType))
            {
                if (contextMap.ContainsKey(treeType))
                {
                    contextMap[treeType].ForEach(x => x.owningContext.behaviourHistory.Clear());
                }
            }
        }
    }
#endif //UNITY_EDITOR
}
