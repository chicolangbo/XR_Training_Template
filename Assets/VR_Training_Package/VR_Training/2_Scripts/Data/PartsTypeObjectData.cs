using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PartsTypeObjectData : MonoBehaviour
{
    public static PartsTypeObjectData instance;
    public List<PartsID> partId_List;

    public List<PartsID> slotToolStand;
    public List<PartsID> slotToolTable;
    public List<PartsID> slotToolBox;
    
    public List<PartsID> ghostToolStand;
    public List<PartsID> ghostToolTable;
    public List<PartsID> ghostToolBox;
    public List<PartsID> ghostPartSlot;
    public List<PartsID> tools;
    public List<PartsID> usingTools;
    public List<PartsID> partsSlots;
    public List<PartsID> wheelSlots;
    public List<PartsID> Icons;
    public List<PartsID> ghostShocks;
    public List<PartsID> partsSlotGhostTable;
    public List<PartsID> ghostArea;
    public PartsID boxSlot; 

    public Material originMat, changeMat;
    public GameObject wheelTire;

    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }


    void Start()
    {
        GetPartsID_List();
        GetTypeListObjects();
        SearchPartsGameObject(226);
    }

    public void SearchPartsGameObject(int num)
    {
        foreach(var part in partId_List)
        {
            if(part.id == num)
            {
                Debug.Log("찾는 파츠 : " + part.gameObject.name);
            }
        }
    }

    void GetTypeListObjects()
    {
        slotToolStand = GetPartsIDByType(EnumDefinition.PartsType.TOOLSTAND_SLOT);
        slotToolTable = GetPartsIDByType(EnumDefinition.PartsType.TOOLTABLE_SLOT);
        slotToolBox = GetPartsIDByType(EnumDefinition.PartsType.TOOLBOX_SLOT);

        ghostToolStand = GetPartsIDByType(EnumDefinition.PartsType.TOOLSTAND_GHOST);
        ghostToolTable = GetPartsIDByType(EnumDefinition.PartsType.TOOLTABLE_GHOST);
        ghostToolBox = GetPartsIDByType(EnumDefinition.PartsType.TOOLBOX_GHOST);
        tools = GetPartsIDByType(EnumDefinition.PartsType.TOOL);
        usingTools = GetPartsIDByType(EnumDefinition.PartsType.USING_TOOL);

        wheelSlots = GetPartsIDByType(EnumDefinition.PartsType.WHEEL_ALIGNMENT_SLOT_GHOST);
        HidePartsObjects(wheelSlots);
        Icons = GetPartsIDByType(EnumDefinition.PartsType.ICON);
        HidePartsObjects(Icons);
        ghostShocks = GetPartsIDByType(EnumDefinition.PartsType.PARTS_SLOT_GHOST_SHOCK);
        HidePartsObjects(ghostShocks);

        partsSlotGhostTable = GetPartsIDByType(EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE);
        StartCoroutine(HideGhostToolTable(partsSlotGhostTable));
        ghostPartSlot = GetPartsIDByType(EnumDefinition.PartsType.PARTS_SLOT);
        StartCoroutine(HideSlotGhost(ghostPartSlot));

        ghostArea = GetPartsIDByType(EnumDefinition.PartsType.PART_GHOST_AREA);
        StartCoroutine(HideGhostArea(ghostArea));

        //box4
        StartCoroutine(HideBox());
    }

    IEnumerator HideBox()
    {
        yield return new WaitForEndOfFrame();
        boxSlot = GetPartIdObject(EnumDefinition.PartsType.BOX,4);
        if(boxSlot)
        if (boxSlot.ghostObject)
        {
            boxSlot.GhostObjectOff();
        }
    }

    IEnumerator HideGhostArea(List<PartsID> parts)
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < parts.Count; i++)
        {
            if(parts[i].ghostObject)
            {
                parts[i].GhostObjectOff();
            }
            
        }
    }

    IEnumerator HideSlotGhost(List<PartsID> parts)
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[i].ghostObject)
            {
                switch (parts[i].id)
                {
                    case P.PLUS_LEAD_LINE_SLOT:
                    case P.MINUS_LEAD_LINE_SLOT:
                    case P.MULTIMETER_GHOST_TABLE:
                    case P.PLUS_LEAD_LINE_BY_ELECTRIC_SCALE_SLOT:
                    case P.MINUS_LEAD_LINE_BY_ELECTRIC_SCALE_SLOT:
                    case P.AMATURE_S_SLOT:
                    case P.AMATURE_L_SLOT:
                    case P.BRUSH_HOLDER_PLATE_SLOT:
                    case P.BRUSH_HOLDER_SLOT:
                    case P.ELECTRIC_MOTOR_B_TERMINAL_SLOT:
                    case P.ELECTRIC_MOTOR_B_TERMINAL_SLOT2:
                    case P.ELECTRIC_MOTOR_B_TERMINAL_SLOT3:
                    case P.VERNIER_CALIPUS_SLOT:
                    case P.ELECTRIC_SCALE_V_BLOCK_SLOT:
                    case P.GROWLER_TESTER_SLOT:
                    case P.BATTERY_TESTER_RED_LEAD_LINE_SLOT:
                    case P.BATTERY_TESTER_BLACK_LEAD_LINE_SLOT:

                    //로어암
                    case 11:
                    case 33:
                    //스트러트어셈블리
                    case 22:
                    case 16:
                    case 17:
                    case 35:
                    case 37:
                    case 38:
                    case 39:
                    case 40:
                    case 41:
                    case 42:
                    case 43:
                    //시동장치
                    case 61:
                    case 74:
                    case 76:
                    case 79:
                    case 101:
                    case 100:
                    case 97:
                    case 98:
                    case 99:
                    case 96:
                    case 95:
                    case 94:
                    case 93:
                    case 92:
                    case 87:
                    case 71:
                    case 70:
                    case 112:
                    case 113:
                    case 202:
                    case 206:
                    case 290:
                    case 319:
                        parts[i].GhostObjectOff();
                        break; 
                }

            }

        }

    }

    IEnumerator HideGhostToolTable(List<PartsID> parts)
    {
        yield return new WaitForEndOfFrame(); 

        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[i].ghostObject)
            {
                parts[i].GhostObjectOff();

                switch (parts[i].id)
                {
                    case P.ELECTRIC_MOTOR_GHOST_TABLE:
                        Transform line = parts[i].transform.Find("digital_multimeter_p_line_01 (1)");
                        if (line) line.gameObject.SetActive(false);
                        break;
                }
            }
            else
            {
                switch (parts[i].id)
                {
                    //시동장치
                    case P.PLUS_JUMP_LINE_GHOST_TABLE:
                    case P.MINUS_JUMP_LINE_GHOST_TABLE:
                    case P.MINUS_JUMP_LINE_GHOST_TABLE2:
                    case P.BATTERY_MINUS_TERMINAL_GHOST_TABLE:
                    case P.ELECTRIC_MOTOR_B_TERMINAL_GHOST_TABLE:
                    case P.BATTERY_PLUS_TERMINAL_GHOST_TABLE:
                    case P.ELECTRIC_MOTOR_B_TERMINAL_GHOST_TABLE2:
                    case P.MINUS_CABLE_GHOST_TABLE1:
                    case P.MINUS_CABLE_GHOST_TABLE2:

                        parts[i].gameObject.SetActive(false);
                        break;
                }
            }
          
        }
    }
    public void DisablePartIdCollider(List<PartsID> parts)
    {
        foreach (var part in parts)
        {
            if (part.myCollider != null && part.myCollider.enabled)
            {
                part.myCollider.enabled = false;
            }
        }
    }

    public void HideGhostObject()
    {
        HidePartsObjects(ghostToolStand);
        HidePartsObjects(ghostToolTable);
       
        if(Secnario_UserContext.instance.currentPlayModeType != EnumDefinition.PlayModeType.EVALUATION)
            HidePartsObjects(ghostToolBox); 

        // HidePartsObjects(ghostPartSlot);
    }

    

    void HidePartsObjects(List<PartsID> partsId)
    {
        for (int i = 0; i < partsId.Count; i++)
        {
            partsId[i].gameObject.SetActive(false);
        }
    }


    void GetPartsID_List()
    {
        partId_List = FindObjectsOfType<PartsID>().ToList();
    }

    public void AddPartsID_Data(PartsID partsID)
    {
        partId_List.Add(partsID);
    }
    public void RemovePartsID_Data(PartsID partsID)
    {
        partId_List.Remove(partsID);
    }

    public void ReplaceID_Data(PartsID originID,PartsID replaceID)
    {
        int partIndex = partId_List.FindIndex(x => x.Equals(originID));
        if(partIndex != -1)
        {
            partId_List[partIndex] = replaceID; 
        }

    }

    public PartsID GetPartIdObject(EnumDefinition.PartsType partsType , int id)
    {
        return partId_List.FirstOrDefault(f => f.partType == partsType && f.id == id);
    }

    public List<PartsID> GetPartsIdObject(EnumDefinition.PartsType partsType, int id)
    {
        return partId_List.Where(w => w.partType == partsType && w.id == id).ToList(); 
    }

    public List<PartsID> GetPartsIDByType(EnumDefinition.PartsType partsType)
    {
        var typeList = partId_List.Where(w => w.partType == partsType).OrderBy(x=> x.id).ToList();
        return typeList;
    }

}
