using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System; 
public abstract class PatternBase : MonoBehaviour
{

    public string id;
    public bool enableEvent = false;
    protected const string LINE1 = "LINE1";
    protected const string LINE2 = "LINE2";

    public abstract void EventStart(Mission_Data missionData);
    public abstract void SetGoalData(Mission_Data missionData);
    public abstract void ResetGoalData();
    public abstract void MissionClear();

    

    public void EnableEvent(bool value)
    {
        enableEvent = value;
    }

    public void SetCurrentMissionID(string _id)
    {
        id = _id;
    }

    public  bool IsMatchPartsID(EnumDefinition.PartsType partsType, string partsName , PartsID partsID)
    {
        return (partsType == partsID.partType && partsName == partsID.partName);
    }
    public bool IsMatchPartsID(EnumDefinition.PartsType partsType,int id,PartsID partsID)
    {
        return (partsType == partsID.partType && id == partsID.id);
    }

    // TODO : 유틸 메소드 유틸리티 클래스로 옮길것.
    /* Utlity Method */

    public bool IsContainController(string tag)
    {
        foreach (var value in System.Enum.GetValues(typeof(EnumDefinition.ControllerType)))
            if (value.ToString() == tag) return true;
        return false;
    }
    public void SetNullObj<T>(T t) where T : UnityEngine.Object
    {
        t = null;
    }
    public void SetNullObj<T>(T t1, T t2) where T : UnityEngine.Object
    {
        t1 = t2 = null;
    }

    public void SetNullObj<T>(T t1, T t2, T t3) where T : UnityEngine.Object
    {
        t1 = t2 = t3 = null;
    }

    public void SetNullObj<T>(List<T> t) where T : UnityEngine.Object
    {
        if(t != null)
        {
            t.Clear();
            t = null;
        }
    }
    public void HightlightOn(PartsID parts)
    {
        parts.highlighter.HighlightOn();
    }

    public void HighlightOff(PartsID parts)
    {
        parts.highlighter.HighlightOff();
    }

    public List<PartsID> GetPartsID_Datas(List<PartsData> partsDatas)
    {
        List<PartsID> datas = new List<PartsID>();
        for (int i = 0; i < partsDatas.Count; i++)
        {
            datas.Add(partsDatas[i].PartsIdObj);
        }
        return datas;
    }

    public void AttachComponent(GameObject obj, EnumDefinition.PartsType type, int partId)
    {
        if (obj.GetComponent<BoxCollider>() == null)
        {
            obj.AddComponent<BoxCollider>();
        }
        if (obj.GetComponent<PartsID>() == null)
        {
            obj.AddComponent<PartsID>().partType = type;
            obj.GetComponent<PartsID>().id = partId;
        }
        if (obj.GetComponent<Scenario_ColliderEvent>() == null)
        {
            obj.AddComponent<Scenario_ColliderEvent>();
        }
        if (obj.GetComponent<Highlighter>() == null)
        {
            obj.AddComponent<Highlighter>().childReneders.Add(obj.GetComponent<MeshRenderer>());
        }

    }


    public EnumDefinition.PatternType GetCurPatternType<T>(T type)
    {
       return (EnumDefinition.PatternType)System.Enum.Parse(typeof(EnumDefinition.PatternType), type.GetType().ToString());
    }
    public void ColliderEnable(PartsID part,bool enable)
    {
        if(part.GetComponent<Collider>() != null)
        {
            part.GetComponent<Collider>().enabled = enable;
            part.GetComponent<Collider>().isTrigger = enable; 
        }
    }

    public void GuideArrowEnable(PartsID part, bool _b)
    {
        GuideArrow ga = part.GetComponent<GuideArrow>();
        if (ga)
        {
            if (_b)
            {
                ga.GuideArrowOn();
            }
            else
            {
                ga.GuideArrowOff();
            }
        }
    }

    public void SocketEnable(PartsID part,bool enable)
    {
        if (part.TryGetComponent(out XRSocketInteractor socket))
        {
            socket.socketActive = enable;
         
           
            //parts_slot 소켓잔상제거
            //todo 호버시 보여줄 오브젝트와 안보여줄 오브젝트 구분필요
            if (
                //현가장치 
                (part.id == P.LOWER_ARM_PIN_SLOT || part.id == P.LOWER_ARM_CASTLE_KNUT_SLOT || 
                part.id == P.LOWER_ARM_WASHER_SLOT || part.id == P.LOWER_ARM_BOLT_SLOT ||
                part.id == P.LOWER_ARM_BOLT_SLOT5 || part.id == P.LOWER_ARM_KNUT_SLOT || 
                part.id == P.WIPER_NUT_COVER_SLOT1 || part.id == P.WIPER_NUT_COVER_SLOT2 || 
                part.id == P.WIPER_NUT_SLOT || part.id == P.WIPER_NUT_SLOT_NEXT ||
                part.id == P.KAWUL_COVER_CLIP_SLOT1 || part.id == P.KAWUL_COVER_CLIP_SLOT2 || 
                part.id == P.KAWUL_COVER_CLIP_SLOT3 || part.id == P.KAWUL_COVER_CLIP_SLOT4 ||
                part.id == P.STRUT_ASSEMBLY_UPPER_BOLT_SLOT || part.id == P.STRUT_ASSEMBLY_UPPER_BOLT_SLOT2 || 
                part.id == P.STRUT_ASSEMBLY_UPPER_BOLT_SLOT1 || part.id == P.WHEEL_SPEED_CENSOR_BRACKET_HOSE_BOLT_SLOT || 
                part.id == P.STABILIZER_LINK_UPPER_NUT_SLOT || part.id == P.STRUT_ASSEMBLY_LOWER_BOLT_SLOT || 
                part.id == P.STRUT_ASSEMBLY_LOWER_NUT_SLOT || part.id == P.STRUT_ASSEMBLY_LOWER_NUT_SLOT2 || 
                part.id == P.STRUT_ASSEMBLY_LOWER_NUT_SLOT1 || part.id == P.ROCK_NUT_SLOT ||
                part.id == P.LOWER_ARM_BOLT_SLOT4 || part.id == P.WIPER_NUT_SLOT1 || part.id == P.WIPER_NUT_SLOT2 || 
                part.id == P.STABILIZER_LINK_UPPER_NUT_SLOT1 ||

                 //시동장치 
                part.id == P.BATTERY_BRACKET_BOLT_SLOT || part.id == P.ELECTRIC_MOTOR_LOWER_BOLT_SLOT || 
                 part.id == P.ELECTRIC_MOTOR_UPPER_BOLT_SLOT || part.id == P.UNDER_COVER_CLIP_SLOT || 
                 part.id == P.ELECTRIC_MOTOR_B_NUT_SLOT || part.id == P.SOLENOID_SWITCH_M_NUT_SLOT || 
                 part.id == P.SOLENOID_SWITCH_BOLT_SLOT1 || part.id == P.SOLENOID_SWITCH_BOLT_SLOT2 || 
                 part.id == P.BRUSH_HOLDER_BOLT_SLOT1 || part.id == P.BRUSH_HOLDER_BOLT_SLOT2 || 
                 part.id == P.ELECTRIC_SCALE_V_BLOCK_SLOT || part.id == P.GROWLER_TESTER_SLOT || 
                  part.id == P.BATTERY_BRACKET_BOLT_SLOT1 || part.id == P.UNDER_COVER_CLIP_SLOT1 || 
                  part.id == P.UNDER_COVER_CLIP_SLOT2 || part.id == P.UNDER_COVER_CLIP_SLOT3 || 
                  part.id == P.UNDER_COVER_CLIP_SLOT4 || part.id == P.UNDER_COVER_CLIP_SLOT5

                )


                && part.partType == EnumDefinition.PartsType.PARTS_SLOT)
            {
                socket.showInteractableHoverMeshes = false;
            }
       
        }
    }

    public void XRGrabEnable(PartsID part,bool enable)
    {
        if (part.GetComponent<XRGrabInteractable>())
        {
            part.GetComponent<XRGrabInteractable>().enabled = enable;
        }

    }

    public void PrintNullExceptionComponetLog(PartsID parts, string text)
    {
        Debug.LogError($"[ {parts.partType} - {parts.id} ] Object 는 패턴 018번에 필요한 {text} Components가 없습니다.");
    }


    public void SetHandIcon(PartsID part, bool enable,int id,Vector3 pos = default,bool parent = false,Vector3 size = default)
    {
        if (Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.EVALUATION)
            return; 

        PartsID hand = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.ICON, id);
        if (hand)
        {
            hand.gameObject.SetActive(enable);
            if (parent)
            {
                hand.transform.SetParent(part.transform);
                hand.transform.localPosition = Vector3.zero + pos;
            }
            else
            {
                hand.transform.position = part.transform.position + pos;
            }
            if (id == 3 || id == 4)
            {
                size = size * 3;
            }
            hand.transform.localScale = size;

            StartCoroutine(LookAtCamera(hand));
        }
    }

    public void HideHandIcon()
    {
        PartsID hand1 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.ICON, 3);
        PartsID hand2 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.ICON, 4);
        PartsID hand3 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.ICON, 5);
        hand1.gameObject.SetActive(false);
        hand2.gameObject.SetActive(false);
        hand3.gameObject.SetActive(false);
        hand1.transform.parent = hand2.transform.parent = hand3.transform.parent = null; 
    }


    IEnumerator LookAtCamera(PartsID part)
    {
        while (part.gameObject.activeSelf)
        {
            yield return null;
            part.transform.LookAt(Camera.main.transform);
        }
    }

    public void LinesOff(PartsID part)
    {
        //SLOT경우
        if ((part.id == P.MINUS_LEAD_LINE 
            || part.id == P.PLUS_LEAD_LINE 
            || part.id == P.BATTERY_TESTER_RED_LEAD_LINE 
            || part.id == P.BATTERY_TESTER_MINUS_LINE

            )
            && 
            part.partType == EnumDefinition.PartsType.PARTS)
        {
            PartsID partslot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, part.id);
            if(partslot)
            {
                Transform line1 = partslot.transform.Find(LINE1);
                if (line1)
                {
                    line1.gameObject.SetActive(false);
                }
                Transform line2 = partslot.transform.Find(LINE2);
                if (line2)
                {
                    line2.gameObject.SetActive(false);
                }
            }
        }

        //고스트테이블경우
        if ((part.id == P.BATTERY_CHARGER_BLACK_LEAD_LINE 
            || part.id == P.BATTERY_CHARGER_RED_LEAD_LINE
            || part.id == P.PLUS_JUMP_LINE
            || part.id == P.MINUS_JUMP_LINE
            || part.id == P.BATTERY_MINUS_TERMINAL_LINE
            || part.id == P.BATTERY_PLUS_TERMINAL_LINE
            || part.id == P.BATTERY_MINUS_TERMINAL_LINE2
            || part.id == P.BATTERY_PLUS_TERMINAL_LINE2
            || part.id == P.BATTERY_MINUS_TERMINAL_LINE3
            || part.id == P.BATTERY_MINUS_TERMINAL_LINE4
            || part.id == P.BATTERY_TESTER_RED_LEAD_LINE
            || part.id == P.BATTERY_TESTER_MINUS_LINE
            )
    &&
    part.partType == EnumDefinition.PartsType.PARTS)
        {
            PartsID ghosttable = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE, part.id); 
            if (ghosttable)
            {
                Transform line1 = ghosttable.transform.Find(LINE1);
                if (line1)
                {
                    line1.gameObject.SetActive(false);
                }
                Transform line2 = ghosttable.transform.Find(LINE2);
                if (line2)
                {
                    line2.gameObject.SetActive(false);
                }
            }
        }


        if(part.id == P.LEAD_LINE && part.partType == EnumDefinition.PartsType.PARTS)
        {
            PartsID growlerslot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, P.GROWLER_TESTER_SLOT);
            Transform line1 = growlerslot.transform.Find(LINE1);
            if(line1)
            {
                line1.gameObject.SetActive(false);
            
            }
        }
        if (part.id == P.LEAD_LINE2 && part.partType == EnumDefinition.PartsType.PARTS)
        {
            PartsID growlerslot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, P.GROWLER_TESTER_SLOT);
            Transform line2 = growlerslot.transform.Find(LINE2);
            if (line2)
            {
                line2.gameObject.SetActive(false);
              
            }
        }

        if (part.id == P.BATTERY_MINUS_TERMINAL_LINE && part.partType == EnumDefinition.PartsType.PARTS)
        {
            PartsID partline = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.BATTERY_MINUS_TERMINAL_LINE);
            Transform line1 = partline.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);
            }
        }
        if (part.id == P.BATTERY_PLUS_TERMINAL_LINE && part.partType == EnumDefinition.PartsType.PARTS)
        {
            PartsID partline = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.BATTERY_PLUS_TERMINAL_LINE);
            Transform line1 = partline.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);
            }
        }

        if (part.id == P.LEAD_LINE && part.partType == EnumDefinition.PartsType.PARTS)
        {
            PartsID partline = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.LEAD_LINE);
            Transform line1 = partline.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);
                line1.GetComponent<MeshRenderer>().enabled = false;
            }

            PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, 153);
            Transform line2 = slot.transform.Find(LINE1);
            if (line2)
            {
                line2.gameObject.SetActive(false);

            }

            PartsID slot2 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, 122);
            Transform line3 = slot.transform.Find(LINE1);
            if (line3)
            {
                line3.gameObject.SetActive(false);

            }

        }

        if (part.id == P.LEAD_LINE2 && part.partType == EnumDefinition.PartsType.PARTS)
        {
            PartsID partline = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.LEAD_LINE2);
            Transform line1 = partline.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);
                line1.GetComponent<MeshRenderer>().enabled = false;
            }

            PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, 152);
            Transform line2 = slot.transform.Find(LINE1);
            if (line2)
            {
                line2.gameObject.SetActive(false);

            }

            PartsID slot2 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, 220);
            Transform line3 = slot.transform.Find(LINE1);
            if (line3)
            {
                line3.gameObject.SetActive(false);

            }
        }



    }

    public void GhostLinesOn(PartsID ghosttable)
    {
        if((ghosttable.id == P.BATTERY_TESTER_RED_LEAD_LINE || ghosttable.id == P.BATTERY_TESTER_MINUS_LINE
            || ghosttable.id == P.BATTERY_TESTER_PLUS_CABLE_GHOST_TABLE2
            || ghosttable.id == P.BATTERY_TESTER_MINUS_CABLE_GHOST_TABLE2
            || ghosttable.id == P.PLUS_JUMP_LINE
            || ghosttable.id == P.MINUS_JUMP_LINE
            || ghosttable.id == P.BATTERY_MINUS_TERMINAL_LINE
            || ghosttable.id == P.BATTERY_PLUS_TERMINAL_LINE
            || ghosttable.id == P.BATTERY_MINUS_TERMINAL_LINE2
            || ghosttable.id == P.BATTERY_PLUS_TERMINAL_LINE2
            || ghosttable.id == P.BATTERY_MINUS_TERMINAL_LINE3
            || ghosttable.id == P.BATTERY_MINUS_TERMINAL_LINE4
            || ghosttable.id == P.MINUS_LEAD_LINE_GHOST_TABLE
            || ghosttable.id == P.PLUS_LEAD_LINE_GHOST_TABLE
            || ghosttable.id == P.BATTERY_CHARGER_RED_LEAD_LINE_GHOST_TABLE
            || ghosttable.id == P.BATTERY_CHARGER_BLACK_LEAD_LINE_GHOST_TABLE
             || ghosttable.id == P.PLUS_JUMP_LINE_GHOST_TABLE
             || ghosttable.id == P.MINUS_JUMP_LINE_GHOST_TABLE
            ) 
            &&
            ghosttable.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
        {
            Transform line1 = ghosttable.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(true); 
            }
        }

        if (ghosttable.id == P.PLUS_JUMP_LINE_GHOST_TABLE
         &&
         ghosttable.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
        {
            PartsID part = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.PLUS_JUMP_LINE);
            Transform line1 = part.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(true);

            }
        }

        if (ghosttable.id == P.MINUS_JUMP_LINE_GHOST_TABLE
        &&
        ghosttable.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
        {
            PartsID part = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.MINUS_JUMP_LINE);
            Transform line1 = part.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(true);

            }
        }


    }

    public void SlotLinesOn(PartsID slot,string part3 = "")
    {
        if((slot.id == P.PLUS_LEAD_LINE_BY_ELECTRIC_SCALE_SLOT 
            || slot.id == P.MINUS_LEAD_LINE_BY_ELECTRIC_SCALE_SLOT
            || slot.id == P.AMATURE_S_SLOT
            || slot.id == P.AMATURE_L_SLOT
            || slot.id == P.LEAD_LINE
            || slot.id == P.LEAD_LINE2
            || slot.id == P.BATTERY_MINUS_TERMINAL_SLOT
            || slot.id == P.ELECTRIC_MOTOR_B_TERMINAL_SLOT
            || slot.id == P.BATTERY_PLUS_TERMINAL_SLOT
            || slot.id == P.ELECTRIC_MOTOR_B_TERMINAL_SLOT2
            || slot.id == P.BATTERY_TESTER_PLUS_CABLE_SLOT
            || slot.id == P.BATTERY_TESTER_RED_LEAD_LINE_SLOT
            || slot.id == P.BATTERY_TESTER_BLACK_LEAD_LINE_SLOT
            )
             &&
            slot.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            Transform line1 = slot.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(true);
            }
        }

        if ((slot.id == P.MINUS_LEAD_LINE_SLOT 
            || slot.id == P.PLUS_LEAD_LINE_SLOT
            )

            && slot.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            if(part3 == LINE1)
            {
                Transform line1 = slot.transform.Find(LINE1); 
                if(line1)
                {
                    line1.gameObject.SetActive(true);

                }
            }
            if (part3 == LINE2)
            {
                Transform line2 = slot.transform.Find(LINE2);
                if (line2)
                {
                    line2.gameObject.SetActive(true); 
                }
            }

        }

    }

    public void GhostLinesOff(PartsID part)
    {
        if (part.id == P.PLUS_LEAD_LINE)
        {
            PartsID ghost = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE, P.PLUS_LEAD_LINE);
            Transform line1 = ghost.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);
       
            }
            Transform line2 = ghost.transform.Find(LINE2);
            if (line2)
            {
                line2.gameObject.SetActive(false);

            }
        }
        if (part.id == P.MINUS_LEAD_LINE)
        {
            PartsID ghost = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE, P.MINUS_LEAD_LINE);
            Transform line1 = ghost.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }
            Transform line2 = ghost.transform.Find(LINE2);
            if (line2)
            {
                line2.gameObject.SetActive(false);

            }
        }


        if (part.id == P.BATTERY_TESTER_RED_LEAD_LINE)
        {
            PartsID ghost = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE, P.BATTERY_TESTER_RED_LEAD_LINE);
            Transform line1 = ghost.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }

        }

        if (part.id == P.BATTERY_TESTER_BLACK_LEAD_LINE)
        {
            PartsID ghost = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE, P.BATTERY_TESTER_BLACK_LEAD_LINE);
            Transform line1 = ghost.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }

        }

        if (part.id == P.BATTERY_CHARGER_RED_LEAD_LINE)
        {
            PartsID ghost = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE, P.BATTERY_CHARGER_RED_LEAD_LINE);
            Transform line1 = ghost.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }
            PartsID ghost2 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE, 208);
            Transform line2 = ghost2.transform.Find(LINE1);
            if (line2)
            {
                line2.gameObject.SetActive(false);

            }

        }

        if (part.id == P.BATTERY_CHARGER_BLACK_LEAD_LINE)
        {
            PartsID ghost = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE, P.BATTERY_CHARGER_BLACK_LEAD_LINE);
            Transform line1 = ghost.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }
            PartsID ghost2 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE, 209);
            Transform line2 = ghost2.transform.Find(LINE1);
            if (line2)
            {
                line2.gameObject.SetActive(false);

            }

        }


        if (part.id == P.PLUS_JUMP_LINE)
        {
            PartsID ghost = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.PLUS_JUMP_LINE);
            Transform line1 = ghost.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }

        }

        if (part.id == P.MINUS_JUMP_LINE)
        {
            PartsID ghost = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.MINUS_JUMP_LINE);
            Transform line1 = ghost.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }

        }

        if (part.id == P.MINUS_CABLE && part.partType == EnumDefinition.PartsType.PARTS)
        {
            PartsID partline = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.MINUS_CABLE);
            Transform line1 = partline.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);
            }
        }
        //1회충전주행실험_차대동력계연결
        if (part.id == 293 || part.id == 294 || part.id == 295)
        {
            PartsID ghost = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, part.id);
            Transform line1 = ghost.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }
        }
    }
    public void SlotLinesOff(PartsID part)
    {
        if (part.id == P.PLUS_LEAD_LINE)
        {
            PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, P.PLUS_LEAD_LINE);
            Transform line1 = slot.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }
            Transform line2 = slot.transform.Find(LINE2);
            if (line2)
            {
                line2.gameObject.SetActive(false);

            }
        }
        if (part.id == P.MINUS_LEAD_LINE)
        {
            PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, P.MINUS_LEAD_LINE);
            Transform line1 = slot.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }
            Transform line2 = slot.transform.Find(LINE2);
            if (line2)
            {
                line2.gameObject.SetActive(false);

            }
        }

        if (part.id == P.BATTERY_TESTER_RED_LEAD_LINE)
        {
            PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, P.BATTERY_TESTER_RED_LEAD_LINE);
            Transform line1 = slot.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }

        }
        if (part.id == P.BATTERY_TESTER_BLACK_LEAD_LINE)
        {
            PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, P.BATTERY_TESTER_BLACK_LEAD_LINE);
            Transform line1 = slot.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }

        }
        if (part.id == P.AMATURE_S_SLOT)
        {
            PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, P.AMATURE_S_SLOT);
            Transform line1 = slot.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }

        }
        if (part.id == P.AMATURE_L_SLOT)
        {
            PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, P.AMATURE_L_SLOT);
            Transform line1 = slot.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }

        }
        if (part.id == P.BRUSH_HOLDER_PLATE_SLOT && part.partType == EnumDefinition.PartsType.TOOL == false)
        {
            PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, P.BRUSH_HOLDER_PLATE_SLOT);
            Transform line1 = slot.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }

        }
        if (part.id == P.BRUSH_HOLDER_SLOT)
        {
            //임시 전체 주석
            //PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, P.BRUSH_HOLDER_SLOT);
            //Transform line1 = slot.transform.Find(LINE1);
            //if (line1)
            //{
            //    line1.gameObject.SetActive(false);

            //}

        }

        //1회충전주행실험_차대동력계연결
        if (part.id == 293 || part.id == 294 || part.id == 295)
        {
            PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, part.id);
            Transform line1 = slot.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);
            }
        }

    }
    public void PartLineEnable(PartsID part1,PartsID part2)
    {
        if (part1.id == P.BATTERY_MINUS_TERMINAL_LINE && part2.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.BATTERY_MINUS_TERMINAL_LINE);
            Transform line1 = slot.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }

        }

        if (part1.id == P.BATTERY_PLUS_TERMINAL_LINE && part2.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.BATTERY_PLUS_TERMINAL_LINE);
            Transform line1 = slot.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }

        }

        if (part1.id == P.MINUS_CABLE2 && part2.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.MINUS_CABLE2);
            Transform line1 = slot.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(false);

            }

        }

        if (part1.id == P.BATTERY_MINUS_TERMINAL_LINE && part2.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
        {
            PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.BATTERY_MINUS_TERMINAL_LINE);
            Transform line1 = slot.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(true);

            }

        }

        if (part1.id == P.BATTERY_PLUS_TERMINAL_LINE && part2.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
        {
            PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.BATTERY_PLUS_TERMINAL_LINE);
            Transform line1 = slot.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(true);

            }

        }

        if (part1.id == P.MINUS_CABLE2 && part2.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
        {
            PartsID slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.MINUS_CABLE2);
            Transform line1 = slot.transform.Find(LINE1);
            if (line1)
            {
                line1.gameObject.SetActive(true);

            }

        }
    }

    public void LeftControllerEnable(bool enable)
    {
        XR_DirectInteractor_Custom[] xrs = FindObjectsOfType<XR_DirectInteractor_Custom>();
        for (int i = 0; i < xrs.Length; i++)
        {
            if (xrs[i].tag == "LeftController")
                xrs[i].enabled = enable;
        }
    }


}

