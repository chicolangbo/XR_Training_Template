using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XR_ControllerBtnEvent : MonoBehaviour
{
    public bool showHandController = false;

    public List<GameObject> controller = new List<GameObject>();
    public GameObject HandModelPrefab;
    public EnumDefinition.ControllerType controllerType;
    InputDevice cont;

    GameObject spawnedContPrefab;
    GameObject spawnedHandModel;
    Animator handAnimator;

    // Update is called once per frame
    void Update()
    {
        if (XR_ControllerBase.instance.isControllerReady)
        {
            cont = XR_ControllerBase.instance.GetInputDeviceController(controllerType);
          
            if(spawnedContPrefab == null)
            {
                spawnedContPrefab = Instantiate(controller[(int)controllerType],transform);
            }
            if(spawnedHandModel == null)
            {
                spawnedHandModel = Instantiate(HandModelPrefab, transform);
                handAnimator = spawnedHandModel.GetComponent<Animator>();
            }

            // setActive model prefab
            spawnedContPrefab.SetActive(!showHandController);
            spawnedHandModel.SetActive(showHandController);




            /*
            if (cont.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryBtnValue) && primaryBtnValue)
            {
                Debug.Log(controllerType + " : pressing primary button");
            }
            // trigger
            if (cont.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
            {
                Debug.Log(controllerType + " : trigger pressed " + triggerValue);
            }
            if (cont.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValues) && primary2DAxisValues != Vector2.zero)
            {
                Debug.Log(controllerType + " : primary Touchpad " + primary2DAxisValues);
            }
            */


            UpdateHandAnimation();
        }
    }

    void UpdateHandAnimation()
    {
        // Trigger Animation
        if (cont.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        // Grip Animation
        if (cont.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }

    }

}
