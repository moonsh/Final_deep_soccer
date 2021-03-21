using UnityEngine;

public class AIEventData
{
    public AIEventData(StimType _type, IEventSource _sourceAgent, float _radius, IEventSource _instigator)
    {
        sourceAgent = _sourceAgent;
        eventInstigator = _instigator;
        sourcePosition = _sourceAgent.GetPosition();
        stimType = _type;
        eventTimeStamp = Time.time;
        radius = _radius;
    }

    public readonly IEventSource sourceAgent;
    public readonly IEventSource eventInstigator;
    public readonly Vector3 sourcePosition;
    public readonly StimType stimType;
    public readonly float eventTimeStamp;
    public readonly float radius;
}
