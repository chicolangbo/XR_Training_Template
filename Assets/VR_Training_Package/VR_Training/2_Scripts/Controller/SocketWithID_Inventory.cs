using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketWithID_Inventory : XRSocketInteractor
{
    public List<PartsID> partsIDList;
    public int matchId;
    [HideInInspector]
    public PartsID selectedPartsId;

    //인벤토리에 들어가는 파츠 id
    int[] idList = {
        //현가장치
        0,1,2,3,4,9,10,12,13,14,15,18,19,20,21,23,24,25,29,30,31,32,
        //시동장치
        78,85,86,88,89,90,91,97,98,99,207,208,209,210,211,212,213,214,226,228,229,230,
        273, 274, 374, 375,       //270, 271, 272,중복
        336,337,338,339,345,346,347,348,349,351,451,
        
        231,232,233,234,235,236,237,238,239,240,241,242,243,244,245,246,248,249,250,251,
        252,253,254,255,256,257,259,260,261,262,270,271,272,274,279,280,281,282,283,284,
        285,286,287,288,275,276,

        388, 390,391,

        400,401,402,403,404,405,406,407,408,409,410,411,        

        342,343,344,

        626,
    };

    private void Start()
    {
        partsIDList = new List<PartsID>();
        StartCoroutine(DelaySetting()); 
    }

    IEnumerator DelaySetting()
    {
        yield return new WaitForEndOfFrame();
        List<PartsID> partsAll = PartsTypeObjectData.instance.GetPartsIDByType(EnumDefinition.PartsType.PARTS);
        for (int i = 0; i < partsAll.Count; i++)
        {
            for (int j = 0;  j < idList.Length;  j++)
            {
                if (partsAll[i].id == idList[j])
                {
                    partsIDList.Add(partsAll[i]);
                    break; 
                }
            }
        }
    }

    private void Update()
    {
        if(partsIDList.Count == 0)
        {
            List<PartsID> partsAll = PartsTypeObjectData.instance.GetPartsIDByType(EnumDefinition.PartsType.PARTS);
            if (PartsTypeObjectData.instance != null && partsAll.Count != 0)
            {
                for (int i = 0; i < partsAll.Count; i++)
                {
                    for (int j = 0; j < idList.Length; j++)
                    {
                        if (partsAll[i].id == idList[j])
                        {
                            partsIDList.Add(partsAll[i]);
                            break;
                        }
                    }
                }
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
            var isMatch = (id.partType == EnumDefinition.PartsType.PARTS && id.id == IsMatchID(id.id) );
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

    int IsMatchID(int id)
    {
        for (int i = 0; i < partsIDList.Count; i++)
        {
            if(partsIDList[i].id == id)
            {
                return id; 
            }
        }

        return 0;
        
    }

    protected override void OnHoverEntering(HoverEnterEventArgs args)
    {
        base.OnHoverEntering(args);
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
            Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnSocketMatchInventory, selectedPartsId);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        var part = other.GetComponent<PartsID>();
        if (part != null)
        {
            Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnSocketTriggerInventory, part);
        }
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