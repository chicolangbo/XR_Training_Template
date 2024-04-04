using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketWith_ID_TYPE : XRSocketInteractor
{
    public PartsID partsID;
    public int matchID;
    public int matchID_b;
    public int matchID_c;
    public EnumDefinition.PartsType matchType;
    public EnumDefinition.PartsType matchType_b;
    public EnumDefinition.PartsType matchType_c;

    [HideInInspector]
    public PartsID selectedPartsId;

    private void Start()
    {
        GetPartsID();
        //호버시 메쉬 간섭제거
        interactableHoverScale = 1.0001f; 
    }

    void GetPartsID()
    {
        if (transform.TryGetComponent<PartsID>(out PartsID partId))
            partsID = partId;
        else
            Debug.LogError(gameObject.name + " - part id  component is null ");
    }

    public override bool CanHover(XRBaseInteractable interactable)
    {
        if(Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.EVALUATION)
        {
            if (!MatchID(interactable))
            {
                // 감점 이벤트 발생
                // Debug.Log("감점 이벤트 발생 " +  interactable.gameObject.name);
                if(Time.time > 10f)
                    Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnToolMissMatchDeductionEvent, partsID);
            }
        }
        return base.CanHover(interactable) && MatchID(interactable);
    }

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        return base.CanSelect(interactable) && MatchID(interactable);
    }


    public bool MatchID(XRBaseInteractable interactable)
    {
        if (interactable.TryGetComponent<PartsID>(out PartsID id))
        {
            var isMatch = (id.partType == matchType && id.id == matchID) || (id.partType == matchType_b && id.id == matchID_b) || (id.partType == matchType_c && id.id == matchID_c);
            if (isMatch)
            {
                selectedPartsId = id;
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    

    protected override void OnHoverEntering(HoverEnterEventArgs args)
    {
        base.OnHoverEntering(args);
        if (selectedPartsId != null)
        {
            if(IsCurMissionDataContainsMyPartsId())
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnSlotSocketHover, selectedPartsId, partsID);
        }
    }
    bool IsCurMissionDataContainsMyPartsId()
    {
        var curParts = Secnario_UserContext.instance.currentAllParts;
        return curParts.Contains(partsID);
    }
    protected override void OnHoverExiting(HoverExitEventArgs args)
    {
        base.OnHoverExiting(args);
        if (selectedPartsId != null)
            selectedPartsId = null;
    }
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        if (selectedPartsId != null)
        {
            if (IsCurMissionDataContainsMyPartsId())
            {
                //효과음
                if (partsID.partType == EnumDefinition.PartsType.TOOLBOX_SLOT &&
                    selectedPartsId.partType == EnumDefinition.PartsType.TOOL)
                {
                    SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.drop_the_tool);
                }
                if (
                    (partsID.partType == EnumDefinition.PartsType.PART_GHOST_AREA &&
                    selectedPartsId.partType == EnumDefinition.PartsType.GROUP_PARTS)
                    ||
                    (partsID.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE &&
                    selectedPartsId.partType == EnumDefinition.PartsType.PARTS)
                    ||
                     (partsID.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE &&
                    selectedPartsId.partType == EnumDefinition.PartsType.GROUP_PARTS)
                    )
                {
                    //효과음 예외처리..
                    if (Secnario_UserContext.instance.GetCurrentPattern() != "P_022" && DoEffect(partsID.id, EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE))
                        SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.put_on_work_table);
                }
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnSlotMatch, selectedPartsId);

            }
               

            Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnSlotMatchSelect, selectedPartsId, partsID);
        }
    }

    bool DoEffect(int id,EnumDefinition.PartsType parttype)
    {
        if(parttype == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
        {
            switch (id)
            {
                case 64:
                case 65:
                case 208:
                case 209:
                    return false; 
            }
        }


        return true; 
    }
}
