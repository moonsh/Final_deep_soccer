using System;

public delegate void OnEventDelegate(AIEventData _event);

public static class EventDefs
{
}

public enum StimType
{
    NONE =0,
    HURT,
    THREATENING_SOUND,
    COUNT
}

[Serializable]
public struct StimRadiusData
{
    public StimType stim;
    public float radius;
}
