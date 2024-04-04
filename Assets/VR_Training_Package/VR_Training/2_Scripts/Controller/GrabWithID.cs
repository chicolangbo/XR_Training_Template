using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabWithID : XRGrabInteractable
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

    public bool MatchID(XRBaseInteractable interactable)
    {
        if (interactable.TryGetComponent<PartsID>(out PartsID id))
        {
            return (id.partType == matchType) && (id.id == partsID.id);
        }
        return false;
    }
    /*
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
    
    
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "TOOLS")
        {
            partsID.transform.GetComponent<GrabWithID>().attachTransform = pivotObj.transform;
            Debug.Log("Hi");
           
        }
    }
    
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "TOOLS")
        {
            partsID.transform.GetComponent<GrabWithID>().attachTransform= pivot_under_Obj.transform;
            Debug.Log("Bye");
        }
    }
    */
}