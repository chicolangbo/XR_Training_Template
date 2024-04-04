using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketWithID_Ratchet : XRSocketInteractor
{
    public PartsID partsID;
    public EnumDefinition.PartsType matchType;
    public Transform wrench;
    public Tools_RatchetWrench_BallJoint evnValue;

    private void Start()
    {
        GetPartsID();
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
    }

    protected override void OnHoverExiting(HoverExitEventArgs args)
    {
        base.OnHoverExiting(args);
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        wrench.transform.GetComponent<XRGrabInteractable>().trackRotation = false;
        wrench.transform.GetComponent<XRGrabInteractable>().trackPosition = false;
        this.transform.GetComponent<SocketWithID_Ratchet>().enabled = false;
    }
}