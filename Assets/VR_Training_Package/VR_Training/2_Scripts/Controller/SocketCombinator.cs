using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketCombinator : XRSocketInteractor
{
    public PartsID partsID;
    PartsID socketParts;
    public int? currentSocketID = null;

    void Start()
    {
        if(partsID == null)
        {
            if (transform.parent.TryGetComponent<PartsID>(out PartsID id))
            {
                partsID = id;
            }
            else
            {
                Debug.LogWarning( this.gameObject.name +  " Parts ID Component 가 없습니다.");
            }
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
            currentSocketID = id.id;
            var isSocket = GlobalData.instance.toolSocketID.Contains(id.id);
            if (isSocket) socketParts = id;
            else socketParts = null;
            return  isSocket;
        }
        return false;
    }

    protected override void OnHoverEntering(HoverEnterEventArgs args)
    {
        base.OnHoverEntering(args);
        SetSocketPartsID();
        // socket match event
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnSocketMatch, socketParts);
        // Scenario_EventManager.instance.socketMatchEvent.Invoke(socketParts);
       // Debug.Log(" Socket Connected : " + currentSocketID );
       // Debug.Log(" Tool Id : " + partsID.id );
    }
       
    protected override void OnHoverExiting(HoverExitEventArgs args)
    {
        base.OnHoverExiting(args);

        //if (args.interactable.TryGetComponent<PartsID>(out PartsID partID))
        //{

        //}
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnSocketSeparate, socketParts);
        //Scenario_EventManager.instance.socketSeparateEvent.Invoke(socketParts);

        if (socketParts != null) socketParts = null;
        SetSocketPartsID();
        currentSocketID = null;
    }

    void SetSocketPartsID()
    {
        if (Secnario_UserContext.instance != null)
        {
            Secnario_UserContext.instance.actionData.cur_socketParts = socketParts;
        }
    }

}