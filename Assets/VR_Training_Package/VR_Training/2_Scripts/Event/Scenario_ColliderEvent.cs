using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario_ColliderEvent : MonoBehaviour
{
   public PartsID partsID;

    void Start()
    {
        if (partsID == null)
            GetPartID();
    }

    void GetPartID()
    {
        if (TryGetComponent<PartsID>(out PartsID _partsID))
        {
            partsID = _partsID;
        }
        else
        {
            Debug.LogError($"{gameObject.name} 에 PartID Component 가 없습니다.");
        }
    }
         

    private void OnTriggerEnter(Collider other)
    {
        Scenario_EventManager.instance.RunEvent( CallBackEventType.TYPES.OnColliderEnter, other, partsID);
        if(IsEvalutionBranchEvent())
        {
            // 평가 터치 
            if(other.tag == "RightController")
            {
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnGrabSelect, partsID, EnumDefinition.ControllerType.RightController);
            }
        }

        //Scenario_EventManager.instance.onColliderEnterEvent.Invoke(other, partsID);
    }

    bool IsEvalutionBranchEvent()
    {
        return Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.EVALUATION && BranchController.instance.enableEvent;
    }

    private void OnTriggerStay(Collider other)
    {
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnColliderStay, other, partsID);
        //Scenario_EventManager.instance.onColliderStayEvent.Invoke(other, partsID);
    }

    private void OnTriggerExit(Collider other)
    {
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnColliderExit, other, partsID);
        //Scenario_EventManager.instance.onColliderExitEvent.Invoke(other, partsID);
    }

}
