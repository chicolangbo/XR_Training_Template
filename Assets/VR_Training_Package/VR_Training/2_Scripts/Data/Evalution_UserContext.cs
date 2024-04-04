using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evalution_UserContext : MonoBehaviour
{

    public static Evalution_UserContext instance;
    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }

    public EnumDefinition.LiftAnimState mainLiftState;
    public EnumDefinition.LiftAnimState centerLiftState;
    public EnumDefinition.HoodState hoodState;

    public bool isEvalutionMissionStart = false;

    void Start()
    {
        
    }

    public void SetMainLiftState(EnumDefinition.LiftAnimState state)
    {
        mainLiftState = state;
    }

    public void SetCenterLiftState(EnumDefinition.LiftAnimState state)
    {
        centerLiftState = state;
    }

    public void SetHoodState(EnumDefinition.HoodState state)
    {
        hoodState = state;
    }

    public EnumDefinition.HoodState GetHoodState()
    {
        return hoodState;
    }

    public EnumDefinition.LiftAnimState GetMainLiftState()
    {
        return mainLiftState;
    }

    public EnumDefinition.LiftAnimState GetCenterLiftState()
    {
        return centerLiftState;
    }

}
