public class AIEventSystem : Singleton<AIEventSystem>
{
    public StimData basicStimData;
    public event OnEventDelegate aiGroupEvent;

    public void PropagateEvents(IEventSource _source, IEventSource _instigator = null, params StimType[] _params)
    {
        foreach (StimType _type in _params)
        {
            float radius = basicStimData.GetRadius(_type);

            if (radius != 0)
            {
                AIEventData eventData = new AIEventData(_type, _source, radius, _instigator);
                aiGroupEvent?.Invoke(eventData);
            }
        }
    }
}
