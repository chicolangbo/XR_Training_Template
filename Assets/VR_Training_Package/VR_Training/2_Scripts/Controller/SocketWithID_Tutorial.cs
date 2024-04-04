using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketWithID_Tutorial : XRSocketInteractor
{
    public PartsID partsID;
    public EnumDefinition.PartsType matchType;
    public GameObject ghostObj;
    [HideInInspector]
    public PartsID selectedPartsId;

    public GameObject wrench_using;
    public GameObject wrench_original;
    public GameObject setingPosition_using;
    public GameObject socket_21mm_original;
    public GameObject socket_21mm;

    public GameObject wrench_addtional_Ghost;
    public GameObject socket_21mm_ghost;
    public GameObject socket_21mm_addtional;

    public float watingTime = 0f;
    public bool hoverOn = false;

    private void Start()
    {
        GetPartsID();
        GetGhostObj();

    }
    void GetPartsID()
    {
        if (transform.TryGetComponent<PartsID>(out PartsID partId))
            partsID = partId;
        else
            Debug.LogError(gameObject.name + " - part id  component is null ");
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
            //var isMatch = (id.partType == EnumDefinition.PartsType.TOOL) && (matchType.ToString().Contains("GHOST")) && (id.id == partsID.id);
            var isMatch = (id.partType == EnumDefinition.PartsType.TOOL) && (id.id == partsID.id);
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
        if (selectedPartsId != null)
        {
            Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnSocketMatch, selectedPartsId);
           // Scenario_EventManager.instance.socketMatchEvent.Invoke(selectedPartsId);
            Debug.Log("ss");
        }
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

        //Invoke("WrenchON", 0.3f);
        this.gameObject.SetActive(false);
        if (wrench_original != null)
        {
            wrench_original.transform.position = this.gameObject.transform.position;
            wrench_original.transform.rotation = this.gameObject.transform.rotation;
            wrench_original.gameObject.SetActive(false);
        }
        if (wrench_using != null)
        {
            wrench_using.transform.position = setingPosition_using.transform.position;
        }
        if (socket_21mm != null)
        {
            socket_21mm.transform.position = setingPosition_using.transform.position;
        }
        if (socket_21mm_ghost != null)
        {
            socket_21mm_ghost.gameObject.SetActive(false);
        }
        if (socket_21mm_original != null)
        {
            socket_21mm_original.transform.position = setingPosition_using.transform.position;
            socket_21mm_original.transform.rotation = setingPosition_using.transform.rotation;
            socket_21mm_original.gameObject.SetActive(false);
        }
        if (socket_21mm_addtional != null)
        {
            socket_21mm_addtional.gameObject.SetActive(false);
        }

    }

    void WrenchON()
    {
        //wrench_using.gameObject.SetActive(true);

        if (wrench_addtional_Ghost != null)
        {
            wrench_addtional_Ghost.gameObject.SetActive(true);
        }
    }
}