using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvalEnableDeductionIUI : MonoBehaviour
{

    public List<GameObject> battery_ui = new List<GameObject>();
    public List<GameObject> electMotor_ui = new List<GameObject>();


    void Start()
    {
        
    }


    void OnEnable()
    {
        if(Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.EVALUATION)
        {
            switch (Secnario_UserContext.instance.currentEvaluationType)
            {
                case EnumDefinition.EVALUATION_TYPE.STATER_BATTERY: EnableUI(battery_ui); break;
                case EnumDefinition.EVALUATION_TYPE.STATER_ELECTRIC_MORTOR: EnableUI(electMotor_ui); break;
            }
        }
    }

    void EnableUI(List<GameObject> uiList)
    {
        foreach (var ui in uiList)
            ui.SetActive(true);
    }
    
    
}
