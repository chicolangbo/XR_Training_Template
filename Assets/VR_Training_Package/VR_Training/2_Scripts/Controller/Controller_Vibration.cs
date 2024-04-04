using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


/// <summary>
///  GRIP 혹은 TRIGGER 버튼을 누르고 있을때 진동 함수 호출
///  LEFT , RIGHT controllerType 으로 설정. 
/// </summary>
public class Controller_Vibration : MonoBehaviour
{
    public EnumDefinition.ControllerType controllerType;
    public EnumDefinition.VibrationBtnType btnType;
    public float amplitude = 1f;

    XRController controller;
    InputDevice devController;
    bool vibrationEnd = false;
    bool isUesing = false;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (!devController.isValid)
            GetControllerDevice();

        if (devController != null)
            Vibration();
    }

    #region Init Method

    void Init()
    {
        GetController();
    }
    
    void GetController()
    {
        controller = UtilityMethod.GetController(controllerType);
    }

    void GetControllerDevice()
    {
        List<InputDevice> devices = new List<InputDevice>();

        var deviceContType = controllerType == EnumDefinition.ControllerType.LeftController ? InputDeviceCharacteristics.Left : InputDeviceCharacteristics.Right;
        var cont = GetController(deviceContType);
        InputDevices.GetDevicesWithCharacteristics(cont, devices);

        if(devices.Count > 0)
            devController = devices[0];
    }

    InputDeviceCharacteristics GetController(InputDeviceCharacteristics type)
    {
        return type | InputDeviceCharacteristics.Controller;
    }

    #endregion


    #region Public Method
    public void SetAmpletude(float value)
    {
        amplitude = value;
    }

    #endregion


    #region Main Method

    void Vibration()
    {
        if (isUesing)
        {
            switch (btnType)
            {
                case EnumDefinition.VibrationBtnType.Trigger: Vibration_Trigger(); break;
                case EnumDefinition.VibrationBtnType.Grap: Vibration_Grip(); break;
            }
        }
    }

    void Vibration_Trigger()
    {
        if(devController.TryGetFeatureValue(CommonUsages.trigger, out float value))
        {
            Vibration(value > 0.1);
        }
    }

    void Vibration_Grip()
    {
        if (devController.TryGetFeatureValue(CommonUsages.gripButton, out bool value))
        {
            Vibration(value);
        }
    }

    void Vibration(bool onOffValue)
    {
        float value = onOffValue == true ? 1 : 0;
        XR_Events.ImpulseController(controller, amplitude, value);
    }

    #endregion








}
