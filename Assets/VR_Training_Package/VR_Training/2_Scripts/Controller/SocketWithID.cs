using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketWithID : XRSocketInteractor
{
    public PartsID partsID;
    public EnumDefinition.PartsType matchType;
    public EnumDefinition.PartsType matchType_b;
    public int matchId;
    public int matchId_b;
    public GameObject ghostObj;
    [HideInInspector]
    public PartsID selectedPartsId;
    public bool isValue = false;
    public bool isHide = false; 

    private void Start()
    {
        if (transform.TryGetComponent<PartsID>(out PartsID partId))
            partsID = partId;
        else
            Debug.LogError(gameObject.name + " - part id  component is null ");

        GetGhostObj();
        //호버시 메쉬 간섭제거
        interactableHoverScale = 1.0001f;
    }

    void GetGhostObj()
    {
        if (ghostObj == null)
        {
            if (transform.childCount > 0)
                ghostObj = transform.GetChild(0).gameObject;
            else
                Debug.LogError(gameObject.name + " - ghost object is null ");
        }
    }

    public override bool CanHover(XRBaseInteractable interactable)
    {
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
            var isMatch = (id.partType == matchType && id.id == matchId ) || (id.partType == matchType_b && id.id == matchId_b);
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
         if (ghostObj != null)
        {
            if(Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.EVALUATION)
                ghostObj.SetActive(false);

           
        }
           
    }

    protected override void OnHoverExiting(HoverExitEventArgs args)
    {
        base.OnHoverExiting(args);
        if (ghostObj != null)
        {
            if(!isHide)
            {
                if (Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.EVALUATION)
                    ghostObj.SetActive(true);
            }

        }
            
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
                if(partsID.partType == EnumDefinition.PartsType.TOOLBOX_SLOT &&
                    selectedPartsId.partType == EnumDefinition.PartsType.TOOL)
                {
                    //SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.drop_the_tool); //에러나서 주석
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
                    if(Secnario_UserContext.instance.GetCurrentPattern() != "P_022")
                        SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.put_on_work_table);
                }
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnSlotMatch, selectedPartsId);
            }
              

            Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnSlotMatchSelect, selectedPartsId, partsID);
        }
    }

    bool IsCurMissionDataContainsMyPartsId()
    {
        var curParts = Secnario_UserContext.instance.currentAllParts;
        return curParts.Contains(partsID);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if (selectedPartsId != null)
        {
            
            //Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnSlotMatchExit, selectedPartsId,partsID);
        }
    }

}