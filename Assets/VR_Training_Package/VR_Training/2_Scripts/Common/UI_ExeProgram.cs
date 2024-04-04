using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ExeProgram : MonoBehaviour
{
    public GameObject Maker;
    public GameObject CarKind;
    public GameObject RunOut;
    
    EnumDefinition.UIClickType clickType = EnumDefinition.UIClickType.Maker;

    public List<UIButton> uiBtn_Maker;
    public List<UIButton> uiBtn_CarKind;

    public GameObject arrow; 

    private void Start()
    {

        for (int i = 0; i < uiBtn_Maker.Count; i++)
        {
            uiBtn_Maker[i].highlight.SetActive(false); 
        }
        for (int i = 0; i < uiBtn_CarKind.Count; i++)
        {
            uiBtn_CarKind[i].highlight.SetActive(false);
        }
        uiBtn_Maker[0].highlight.SetActive(true);
        uiBtn_CarKind[0].highlight.SetActive(true);
        SetClickType(clickType); 
    }

    public void SetClickType(EnumDefinition.UIClickType type)
    {
        clickType = type;
        switch (clickType)
        {
            case EnumDefinition.UIClickType.Maker:
                Maker.SetActive(true);
                CarKind.SetActive(false);
                RunOut.SetActive(false);
                break;
            case EnumDefinition.UIClickType.CarKind:
                Maker.SetActive(false);
                CarKind.SetActive(true);
                RunOut.SetActive(false);
                break;
            case EnumDefinition.UIClickType.RunOut:
                Maker.SetActive(false);
                CarKind.SetActive(false);
                RunOut.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ClickBtn_Maker(UIButton btn)
    {
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnUISelect, btn);
    }

    public void ClickBtn_CarKind(UIButton btn)
    {
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnUISelect, btn);
    }
}
