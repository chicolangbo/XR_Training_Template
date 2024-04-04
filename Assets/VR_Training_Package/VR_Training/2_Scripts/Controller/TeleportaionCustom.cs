using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class TeleportaionCustom : TeleportationArea
{
    public PartsID partsID;
    public EnumDefinition.PartsType matchType;
    public GameObject pivotObj;
    public GameObject pivot_under_Obj;

    void Start()
    {
        GetPartsID();
        GetPivotObj();
    }

    void Update()
    {

    }

    void GetPartsID()
    {
        if (transform.TryGetComponent<PartsID>(out PartsID partId))
            partsID = partId;
        else
            Debug.LogError(gameObject.name + " - part id  component is null ");

    }
    
    void GetPivotObj()
    {
        if (pivotObj == null || pivot_under_Obj == null)
        {
            if (transform.childCount > 0)
            {
                pivotObj = transform.GetChild(0).gameObject;
                pivot_under_Obj = transform.GetChild(1).gameObject;
            }
            else
                Debug.LogError(gameObject.name + " - pivot object is null ");
        }
    }

    /*
    protected virtual void Grab(XRBaseInteractable interactable)
    {
        return base.Grab(interactable);
    }*/

    public bool MatchID(XRBaseInteractable interactable)
    {
        if (interactable.TryGetComponent<PartsID>(out PartsID id))
        {
            return (id.partType == matchType) && (id.id == partsID.id);
        }
        return false;
    }
    protected override void OnHoverEntering(HoverEnterEventArgs args)
    {
        
    }
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        partsID.transform.GetComponent<GrabWithID>().attachTransform = pivot_under_Obj.transform;
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        partsID.transform.GetComponent<GrabWithID>().attachTransform = pivotObj.transform;

    }
}