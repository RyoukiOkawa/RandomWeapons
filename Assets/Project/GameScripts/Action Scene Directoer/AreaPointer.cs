using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaPointer : Component
{
    
}

[System.Serializable]
public class StateArea
{
    [SerializeField] private AreaPointer m_stateAreaPointer = null;
    [SerializeField] private AnimationCurve zRootCurve;
    [SerializeField] private AnimationCurve xRootCurve;

    public AreaPointer StateAreaPointer { get => m_stateAreaPointer;}

    public AnimationCurve XRootCurve { get => xRootCurve;}
    public AnimationCurve ZRootCurve { get => zRootCurve;}
}