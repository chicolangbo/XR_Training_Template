using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketWithID_Parts : XRSocketInteractor
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

    private void Start()
    {
        if (transform.TryGetComponent<PartsID>(out PartsID partId))
            partsID = partId;
        else
            Debug.LogError(gameObject.name + " - part id  component is null ");
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
            var isMatch = (id.partType == matchType && id.id == matchId) || (id.partType == matchType_b && id.id == matchId_b);
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
            ghostObj.SetActive(false);
    }

    protected override void OnHoverExiting(HoverExitEventArgs args)
    {
        base.OnHoverExiting(args);
        if (ghostObj != null)
            ghostObj.SetActive(true);
        if (selectedPartsId != null)
            selectedPartsId = null;
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnSlotMatch, selectedPartsId);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if (selectedPartsId != null)
        {
           
        }
        
    }
}