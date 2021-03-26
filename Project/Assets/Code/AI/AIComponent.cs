using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class AIComponent : MonoBehaviour, IEventSource
{
    public BehaviourTreeType behaviourTreeType;
    public SensorySystem sensorySystem;
    public AIEventHandler eventHandler;
    public Rigidbody rb;
    public Transform goal;
    public Transform ball;
    public GameObject[] opponents; //forward opponents
    public GameObject[] teammates;

    internal AIState currentState = AIState.IDLE;
    internal IEventSource currentTarget;

    Animator animatorController;
    NavMeshAgent navAgent;
    BTContext aiContext;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animatorController = GetComponent<Animator>();

        aiContext = new BTContext(this, animatorController, navAgent, rb, goal, ball, opponents, teammates);
    }

    private void Start()
    {
        sensorySystem.Initialize(this, navAgent);
        eventHandler.Initialize(this, animatorController, navAgent);
        BehaviourTreeRuntimeData.RegisterAgentContext(behaviourTreeType, aiContext);
    }

    void Update()
    {
        sensorySystem.Update();
        eventHandler.Update();
    }

    void OnDestroy()
    {
        eventHandler.OnDestroy();
        BehaviourTreeRuntimeData.UnregisterAgentContext(behaviourTreeType, aiContext);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
