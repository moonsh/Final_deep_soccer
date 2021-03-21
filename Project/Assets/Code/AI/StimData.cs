using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StimData", menuName = "AIData/new Stim Data", order = 0)]
public class StimData : ScriptableObject
{
    public List<StimRadiusData> stimRadiusData;

    internal float GetRadius(StimType _type)
    {
        StimRadiusData data = stimRadiusData.Find(x => x.stim == _type);
        return data.radius;
    }
}
