using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class XR_DirectInteractor_Custom : XRDirectInteractor
{
    public EnumDefinition.ControllerType controllerType;
    PartsID grabParts;
    PartsID prevParts;
    static string firstGrap = ""; 

    void AdjustPos(PartsID part,bool exit = false)
    {
        if(part.tag == "Adjust")
        {
            Transform attach = part.transform.Find("Attach");
            if(attach)
            {
                switch(part.id)
                {
                    //멀티미터
                    case 125:
                        attach.transform.localEulerAngles = new Vector3(180, 0, 180);
                        break;
                    //바테리테스터
                    case 14:
                        attach.transform.localEulerAngles = new Vector3(180, 0, 180);
                        break;
                        //배터리 브라켓
                    case 70:
                        attach.transform.localEulerAngles = new Vector3(180, 180, 0);
                        break;
                        //배터리 테스터기 적색리드선
                    case 108:
                        attach.transform.localPosition = new Vector3(0, 0.1f, 0);
                        break;
                        //배터리 테스터기 흑색리드선
                    case 109:
                        attach.transform.localPosition = new Vector3(0, 0.1f, 0);
                        break;
                        //배터리 충전기 적색리드선
                    case 110:
                        attach.transform.localPosition = new Vector3(0.6f, 0, 0);
                        attach.transform.localEulerAngles = new Vector3(90, 90, 0);
                        break;
                        //배터리 충전기 흑색리드선  
                    case 111:
                        attach.transform.localPosition = new Vector3(0.5f, 0, 0);
                        attach.transform.localEulerAngles = new Vector3(90, 90, 0);
                        break;
                    case 112:
                    case 113:
                        attach.transform.localPosition = new Vector3(0, 0, -0.11f);
                        attach.transform.localEulerAngles = new Vector3(270, 0, 0);
                        break;
                }

                if(exit)
                {
                    attach.transform.localEulerAngles = Vector3.zero;
                    attach.transform.localPosition = Vector3.zero; 
                }
            }

            //연장대
            if(part.id == 30)
            {
                if(exit)
                {
                    part.GetComponent<BoxCollider>().enabled = false;
                    part.GetComponent<XRGrabInteractable>().attachTransform = part.transform.GetChild(1);
                    part.GetComponent<BoxCollider>().enabled = true;
                }
                else
                {
                    part.GetComponent<BoxCollider>().enabled = false;
                    part.GetComponent<XRGrabInteractable>().attachTransform = part.transform.GetChild(0);
                    part.GetComponent<BoxCollider>().enabled = true;
                }

            }
            
        }
    }


    void TemporaryDisableGrab(PartsID part)
    {
        
        if(part.partType == EnumDefinition.PartsType.TOOL)
        {
           // bool leftgrip = XR_ControllerBase.instance.GetGripStatusLeft();
           // bool rightgrip = XR_ControllerBase.instance.GetGripStatusRight(); 

           // if(leftgrip || rightgrip)
            {
                if(firstGrap == "")
                {
                    if (transform.name == "RightHand Controller")
                    {
                        firstGrap = "right"; 
                    }
                    if (transform.name == "LeftHand Controller")
                    {
                        firstGrap = "left"; 
                    }
                }
                else if(firstGrap == "right")
                {
                    if (transform.name == "LeftHand Controller")
                    {
                        Debug.Log("left");
                        part.GetComponent<BoxCollider>().enabled = false;
                        part.GetComponent<XRGrabInteractable>().enabled = false;
                        StartCoroutine(EnableBoxCollider(part));
                    }
                }
                else if(firstGrap == "left")
                {
                    if (transform.name == "RightHand Controller")
                    {
                        Debug.Log("right");
                        part.GetComponent<BoxCollider>().enabled = false; 
                        part.GetComponent<XRGrabInteractable>().enabled = false;
                        StartCoroutine(EnableBoxCollider(part));
                    }
                }

            }
      

        }
    }
    
    IEnumerator EnableBoxCollider(PartsID part)
    {
        yield return new WaitForEndOfFrame(); 
        part.GetComponent<BoxCollider>().enabled = true;
        part.GetComponent<XRGrabInteractable>().enabled = true;
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        DoorActionEnter(args); 
        if (args.interactableObject.transform.TryGetComponent<PartsID>(out PartsID partID))
        {
            grabParts = partID;
            AdjustPos(partID);
            //한손에 툴쥐고있을때 다른손으로 끌려가는것 방지
            TemporaryDisableGrab(partID); 
            //partID.transform.localEulerAngles = new Vector3(0, 90, 0);
            Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnGrabSelect, partID, controllerType);
            //Scenario_EventManager.instance.grabInteractableSelectEvent.Invoke(partID, controllerType);
            SetPartsID();
            // Debug.Log("Current Grab Parts : " + grabParts.gameObject.name);
            prevParts = partID;
        }
    }
    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        DoorActionExit(args);
        if (grabParts != null)
        {
            //Debug.Log("111111  ====> " + firstGrap + " =====> " + XR_ControllerBase.instance.GetGripStatusRight());
            if (transform.name == "RightHand Controller" && firstGrap == "right")
            {
                if(XR_ControllerBase.instance.GetGripStatusRight() == false)
                {
                    firstGrap = ""; 
                }
            }
            if (transform.name == "LeftHand Controller" && firstGrap == "left")
            {
                if (XR_ControllerBase.instance.GetGripStatusLeft() == false)
                {
                    firstGrap = "";
                }
            }


            AdjustPos(grabParts, true);
            Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnGrabExit, grabParts, controllerType);
            grabParts = null; 
        }
        SetPartsID();
    }
    //public void ExitEvent()
    //{
    //    if (grabParts != null) grabParts = null;
    //     SetPartsID();
    //    Scenario_EventManager.instance.grabInteractableExitEvent.Invoke(prevParts, controllerType);
    //}

    void DoorActionEnter(SelectEnterEventArgs args)
    {

        if (args.interactableObject.transform)
        {
            if (args.interactableObject.transform.name == "Handle")
            {
                GameObject door = GameObject.Find("leftdoorfront");  
                CheckDoorAngle doorAngle = door.GetComponent<CheckDoorAngle>(); 
                door.GetComponent<Rigidbody>().isKinematic = false; 
                doorAngle.IsDoorMoving = true;

            }
        }

    }

    void DoorActionExit(SelectExitEventArgs args)
    {

        if (args.interactableObject.transform)
        {
            if (args.interactableObject.transform.name == "Handle")
            {
                GameObject door = GameObject.Find("leftdoorfront");
                CheckDoorAngle doorAngle = door.GetComponent<CheckDoorAngle>();
                doorAngle.IsDoorMoving = false;
                door.GetComponent<Rigidbody>().isKinematic = true;
                args.interactable.transform.position = GameObject.Find("HandleParent").transform.position;

            }
        }


    }



    void SetPartsID()
    {
        //ExitEvent();
        if (Secnario_UserContext.instance != null)
        {
            if (controllerType == EnumDefinition.ControllerType.RightController)
                Secnario_UserContext.instance.actionData.cur_r_grabParts = grabParts;
            else
                Secnario_UserContext.instance.actionData.cur_l_grabParts = grabParts;
        }
    }


}
