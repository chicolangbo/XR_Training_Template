using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XR_ControllerBase : MonoBehaviour
{
    public static XR_ControllerBase instance;
    // left , right
    List<InputDevice> inputDeviceControllers = new List<InputDevice>();
    // left , right
    List<ActionBasedController> controllers = new List<ActionBasedController>();
    // controller animator
    //List<Animator> controllerAnimator = new List<Animator>();

    public bool isControllerReady = false;

    InputDevice leftCont;
    InputDevice rightCont;

    bool gripValueRight, gripValueLeft;
    bool xValue;
    bool xButtonCheck = true;
    bool yButtonCheck = true;
    bool lToggle = true;
    bool rToggle = true;
    public bool isOnOculus; // true = ��ŧ����, false = vive

    //ui control 
    public XRController uiControl;

    const string OCULUS = "Oculus";
    public GameObject[] ViveUI;

    public SystemUI systemUI;
    public GameObject joysticTutorial;

    
    private bool alreadyToggled1, alreadyToggled2;

    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }

    void Start()
    {
        Init();        
    }
    void Init()
    {
        SetInputDeviceController(EnumDefinition.ControllerType.RightController); // device
        if(uiControl != null)
            uiControl.gameObject.SetActive(false); 
    }

    //void GetControllerAnimator()
    //{
    //    if (controllers.Count > 0)
    //    {
    //        foreach (var cont in controllers)
    //        {
    //            if (cont.GetComponentInChildren<Animator>() != null)
    //                controllerAnimator.Add(cont.GetComponentInChildren<Animator>());
    //            else
    //            {
    //                Debug.LogError("xr controller �� animator component�� �����ϴ�.");
    //                controllerAnimator.Add(new Animator());
    //            }

    //        }
    //    }
    //}

    InputDevice SetInputDeviceController(EnumDefinition.ControllerType controllerType)
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDeviceCharacteristics controller;
        controller = controllerType == EnumDefinition.ControllerType.LeftController ? InputDeviceCharacteristics.Left : InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(controller, devices);
        if (devices.Count > 0)
        {
            //Debug.LogError("���� ��� : " + devices[0].manufacturer);
            //��ŧ�������� ���̺�����..
            if (devices[0].manufacturer == OCULUS)
            {
                Debug.Log(OCULUS);
                isOnOculus = true;
            }
            else
            {
                ModifyViveUI();
                // ���̺� ui����
                Debug.Log("vive");
                isOnOculus = false;
            }
            return devices[0];
        }
           
        else
            return new InputDevice();
    }

    private void Update()
    {
        if (!leftCont.isValid)
            leftCont = SetInputDeviceController(EnumDefinition.ControllerType.LeftController);
        if (!rightCont.isValid)
            rightCont = SetInputDeviceController(EnumDefinition.ControllerType.RightController);

        //if (leftCont.isValid && rightCont.isValid && inputDeviceControllers.Count <= 0)
        if(inputDeviceControllers.Count <= 0 || controllers.Count <=0)
        {
            Debug.Log("xr_controllerBase");
            inputDeviceControllers.Add(leftCont);
            inputDeviceControllers.Add(rightCont);
            SetControllers(); // scene controller
            //GetControllerAnimator(); // controller animator
            isControllerReady = true;

            //foreach(var controller in inputDeviceControllers)
            //{
            //    Debug.Log(" dev name : " + controller.name);
            //}
        }

        //�׸���ư Ȯ��
        if (rightCont.TryGetFeatureValue(CommonUsages.gripButton, out gripValueRight) && gripValueRight) {
        }
        //�׸���ư Ȯ��
        if (leftCont.TryGetFeatureValue(CommonUsages.gripButton, out gripValueLeft) && gripValueLeft) {
        }


        Right_ButtonUpdate();
        //ī�޶�ui toggle
        //CameraUIToggle();

        //height adjust
        //XRRigYAdjust();

    }

    void Right_ButtonUpdate()
    {
        if (rightCont.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPrimary))    //A
        {
            if (rightPrimary)
            {
                if (!alreadyToggled1)
                {
                    if (rToggle)
                    {
                        systemUI.OpenSystemMenuUI(true);
                        rToggle = !rToggle;
                        joysticTutorial.SetActive(false);
                    }
                    else
                    {
                        systemUI.OpenSystemMenuUI(false);
                        rToggle = !rToggle;
                    }
                    alreadyToggled1 = true;
                }
            }
            else
            {
                alreadyToggled1 = false;
            }
        }


        if (rightCont.TryGetFeatureValue(CommonUsages.secondaryButton, out bool rightSec))  //B
        {
            if (rightSec)
            {
                if (!alreadyToggled2)
                {
                    if (lToggle)
                    {
                        Secnario_UserContext.instance.EnableCameraUI(true);
                        lToggle = !lToggle;
                        joysticTutorial.SetActive(false);
                    }
                    else if (!lToggle)
                    {
                        Secnario_UserContext.instance.EnableCameraUI(false);
                        lToggle = !lToggle;
                    }
                    alreadyToggled2 = true;
                }
            }
            else
            {
                alreadyToggled2 = false;
            }
        }

    }

    void CameraUIToggle()
    {
        bool test = false;
        if(leftCont.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool value2))
        {
            test = value2;
        }
        

        if (leftCont.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 value))
        {
            if (!test && isOnOculus == false)
                return;

            if (value.x >= 0.5 && xButtonCheck)
            {
         
                xButtonCheck = false;
                Invoke("xButtonCheckEnable", 0.2f);
                if (lToggle)
                {
                    Secnario_UserContext.instance.EnableCameraUI(true);
                    lToggle = !lToggle;
                    joysticTutorial.SetActive(false);
                }
                else
                {
                    Secnario_UserContext.instance.EnableCameraUI(false);
                    lToggle = !lToggle;
                }
            }
            if (value.x <= -0.5 && yButtonCheck)
            {
               
                yButtonCheck = false;
                Invoke("yButtonCheckEnable", 1f);
                if (rToggle)
                {
                    systemUI.OpenSystemMenuUI(true);
                    rToggle = !rToggle;
                    joysticTutorial.SetActive(false);
                }
                else
                {
                    systemUI.OpenSystemMenuUI(false);
                    rToggle = !rToggle;
                }
            }
        }





        /*
        //����x��ư Ȯ��
        if (xButtonCheck)
        {
            if (leftCont.TryGetFeatureValue(CommonUsages.primaryButton,
                              out xValue) && xValue)
            {
                xButtonCheck = false;
                xToggle = !xToggle;
                Invoke("xButtonCheckEnable", 0.2f);
                if(xToggle)
                {
                    Secnario_UserContext.instance.EnableCameraUI(true); 
                }
                else
                {
                    Secnario_UserContext.instance.EnableCameraUI(false);
                }

            }
        }
        if(yButtonCheck)
        {
            if (leftCont.TryGetFeatureValue(CommonUsages.secondaryButton, out bool bValue))
            {
                if(bValue)
                {
                    yButtonCheck = false;
                    Invoke("yButtonCheckEnable", 0.2f);
                    if (yToggle)
                    {
                        systemUI.OpenSystemMenuUI(true);
                        yToggle = !yToggle;
                    }
                    else
                    {
                        systemUI.OpenSystemMenuUI(false);
                        yToggle = !yToggle;
                    }
                }
            }
        }
        */
    }

    public void ToggleReset()
    {
        rToggle = true;  
    }

    GameObject xrRig;
    void XRRigYAdjust()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            xrRig = GameObject.Find("XR Rig"); 
          if(xrRig)
            {
                Vector3 pos = xrRig.transform.position;
                pos.y += 0.1f;
                xrRig.transform.position = pos; 
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            xrRig = GameObject.Find("XR Rig");
            if (xrRig)
            {
                Vector3 pos = xrRig.transform.position;
                pos.y -= 0.1f;
                xrRig.transform.position = pos;
            }
        }
    }

    void xButtonCheckEnable()
    {
        xButtonCheck = true; 
    }

    void yButtonCheckEnable()
    {
        yButtonCheck = true;
    }

    public bool GetXToggle()
    {
        return lToggle; 
    }


    void SetControllers()
    {
        //controllers.Add(UtilityMethod.GetController(EnumDefinition.ControllerType.LeftController));
        //controllers.Add(UtilityMethod.GetController(EnumDefinition.ControllerType.RightController));
        //if(controllers.Count <= 0)
        //{
        if(GameObject.FindGameObjectWithTag("LeftController") != null || GameObject.FindGameObjectWithTag("RightController") != null)
        {
            controllers.Add(GameObject.FindGameObjectWithTag("LeftController").GetComponent<ActionBasedController>());
            controllers.Add(GameObject.FindGameObjectWithTag("RightController").GetComponent<ActionBasedController>());
        }
        //}
    }

    public ActionBasedController GetController(EnumDefinition.ControllerType controllerType)
    {
        Debug.Log($"��Ʈ�ѷ� Ÿ�� : {(int)controllerType}, ��Ʈ�ѷ� ���� : {controllers.Count}");
        return controllers[(int)controllerType];
    }

    public InputDevice GetInputDeviceController(EnumDefinition.ControllerType controllerType)
    {
        return inputDeviceControllers[(int)controllerType];
    }

    /// <summary>
    /// ������� ���� ���� ���� �Ͻñ� �ٶ��ϴ�.
    /// </summary>
    public Animator GetControllerAnimator(EnumDefinition.ControllerType controllerType)
    {
        return new Animator(); //controllerAnimator[(int)controllerType];
    }
    
    /// <summary>
    /// �׸���ư �������� ����
    /// </summary>
    public bool GetGripStatusRight()
    {
        return gripValueRight; 
    }

    public bool GetGripStatusLeft()
    {
        return gripValueLeft;
    }

    public (ActionBasedController cont, UnityEngine.XR.InputDevice inputDevice,bool isGripedRight,bool isGripedLeft,string tag) IsGrip(Collider col)
    {
        (ActionBasedController cont, UnityEngine.XR.InputDevice inputDevice, bool isGripedRight, bool isGripedLeft,string tag) elements;
        if (col.tag == "RightController")
        {
            elements.cont = GetController(EnumDefinition.ControllerType.RightController);
            elements.inputDevice = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.RightController);
            
        }
        else
        {
            elements.cont = GetController(EnumDefinition.ControllerType.LeftController);
            elements.inputDevice = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.LeftController);
        }

        elements.isGripedRight = GetGripStatusRight();
        elements.isGripedLeft = GetGripStatusLeft();
        elements.tag = col.tag;
        Debug.Log($"is grip �Լ� �۵� : {elements}");
        return elements;
    }

    public void ModifyViveUI()
    {
        if (ViveUI == null)
            return;
        for (int i = 0; i < ViveUI.Length; i++)
        {
            Vector3 temp = ViveUI[i].transform.localPosition;
            temp.y = -600;
            ViveUI[i].transform.localPosition = temp;
        }       
    }
}
