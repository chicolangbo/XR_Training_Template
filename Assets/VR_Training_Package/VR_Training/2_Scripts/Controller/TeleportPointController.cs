using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TeleportPointController : MonoBehaviour
{
    public Transform teleportPoint;
    public GameObject reticleSet;
    public GameObject controller;
    public float moveSpeed = 0.04f, camera_distance = 0f, line_Height = -0.38f;
    public EnumDefinition.ControllerType controllerType;
    InputDevice cont;

    //test parts
    public float lineRenderOffTime = 2f;
    private float delayTime = 0f;
    private int num = 0;

    bool isOnTriggerPointer;
    bool isOnLineRender = false;

    Vector3 pos;
    public Transform cam;
    private Quaternion addRot, newRotation;

    //Color Test Parts
    public GameObject rayLine;
    public GameObject reticle;
    Renderer capsuleColor;
    Renderer liner;

    public LineRenderer lineRender;

    void Start()
    {
        teleportPoint.transform.position = new Vector3(controller.transform.position.x, line_Height, controller.transform.position.z) + teleportPoint.transform.forward * 1.5f;
        capsuleColor = reticle.GetComponent<Renderer>();
        liner = rayLine.GetComponent<Renderer>();

        StartCoroutine(HideLineRender());
    }

    // Update is called once per frame
    void Update()
    {
        if (XR_ControllerBase.instance.isControllerReady)
        {
            cont = XR_ControllerBase.instance.GetInputDeviceController(controllerType);

            switch (num)
            {
                case 0:
                    GetDelay();
                    break;
                case 1:
                    UpdateTelepointPosition();
                    if (XR_ControllerBase.instance.isOnOculus == false)
                        UpdateVive();
                    break;
                case 2:
                    break;
            }
        }
    }

    void GetDelay()
    {
        if (delayTime < 0.5f)
            delayTime += Time.deltaTime;
        if (delayTime >= 0.5f)
            num = 1;
    }
    
    void UpdateVive()
    {
        if (cont.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool value))
        {
            isOnTriggerPointer = value;
        }
    }

    void UpdateTelepointPosition()
    {
        // get pad point 
        // primary2DAxis out Vector2 value
        if (cont.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 value))
        {
            /*
            var direction = cam.transform.forward;
            var rot = Quaternion.LookRotation(direction);
            var lookRot = Quaternion.Euler(teleportPoint.rotation.eulerAngles.x, rot.eulerAngles.y, teleportPoint.rotation.eulerAngles.z);
            reticleSet.transform.rotation = lookRot;
            */
            if (value != Vector2.zero)
            {
                lineRender.enabled = true;
                isOnLineRender = true;
            }
            else
                isOnLineRender = false;
            addRot = Quaternion.Euler(new Vector3(teleportPoint.rotation.eulerAngles.x, GameObject.Find("Player").transform.localRotation.y, teleportPoint.rotation.eulerAngles.z));
            reticleSet.transform.localRotation = addRot;


            if (XR_ControllerBase.instance.isOnOculus == true || XR_ControllerBase.instance.isOnOculus == false && isOnTriggerPointer)
            {
                pos = controller.transform.rotation.eulerAngles;
                teleportPoint.transform.rotation = Quaternion.Euler(0, pos.y, 0);
                if (value != Vector2.zero)
                {
                    camera_distance = Vector3.Distance(GameObject.FindWithTag("MainCamera").transform.position, teleportPoint.transform.position);
                    //teleportPoint.Translate(new Vector3(value.x * 1.5f, 0, value.y) * moveSpeed);
                    Vector3 move = new Vector3(value.x * 1.5f, 0, value.y * 1.5f) ;
                    teleportPoint.Translate(move * moveSpeed);

                    // 거리 제한
                    if (camera_distance >= 1f && camera_distance <= 6.0f)
                    {

                    }
                    else
                    {
                        teleportPoint.transform.position = new Vector3(GameObject.FindWithTag("MainCamera").transform.position.x, line_Height, GameObject.FindWithTag("MainCamera").transform.position.z) + teleportPoint.transform.forward * 1.5f;
                    }

                    capsuleColor.material.color = Color.blue;
                    liner.material.color = Color.blue;
                }
                else if (value == Vector2.zero)
                {
                    //teleportPoint.transform.position = new Vector3(controller.transform.position.x, line_Height, controller.transform.position.z) + teleportPoint.transform.forward * 1.5f;

                    capsuleColor.material.color = new Color32(0, 0, 255, 100);
                    liner.material.color = new Color32(0, 0, 255, 100);
                }
            }
            /*
            if (cont.TryGetFeatureValue(CommonUsages.primaryButton, out bool primary) && value == Vector2.zero)//&& teleportPoint.GetComponent<Renderer>().material.color == Color.blue)
            {
                if (primary)
                {
                    GameObject.FindWithTag("XRrig").transform.position = teleportPoint.transform.position;
                    teleportPoint.transform.position = new Vector3(GameObject.FindWithTag("XRrig").transform.position.x, line_Height, GameObject.FindWithTag("XRrig").transform.position.z) + teleportPoint.transform.forward * 1.5f;
                    num = 0;
                    delayTime = 0f;
                    capsuleColor.material.color = new Color32(0, 0, 255, 100);
                    liner.material.color = new Color32(0, 0, 255, 100);
                }
            }*/
            if (cont.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTrigger))// && value == Vector2.zero  / && teleportPoint.GetComponent<Renderer>().material.color == Color.blue)
            {
                if (isTrigger && lineRender.enabled)
                {
                    // Set Clamp Area
                    teleportPoint.transform.position = ClampArea(teleportPoint.transform.position);

                    if (XR_ControllerBase.instance.isOnOculus)
                    {
                        Vector3 camPos = GameObject.FindWithTag("MainCamera").transform.localPosition;
                        camPos.y = 0;
                        GameObject.FindWithTag("XRrig").transform.position = teleportPoint.transform.position - camPos;
                    }
                    else
                        GameObject.FindWithTag("XRrig").transform.position = teleportPoint.transform.position;


                    num = 0;
                    delayTime = 0f;
                    capsuleColor.material.color = new Color32(0, 0, 255, 100);
                    liner.material.color = new Color32(0, 0, 255, 100);
                    lineRender.enabled = false;
                }
            }

        }
    }

    //Mathf.clamp
    Vector3 ClampArea(Vector3 pos)
    {// -6(x), 10(z)    /   
        return new Vector3(Mathf.Clamp(pos.x, -6, 6), line_Height, Mathf.Clamp(pos.z, -10, 10));
    }

    IEnumerator HideLineRender()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            yield return new WaitUntil(() => !isOnLineRender);
            yield return new WaitForSeconds(lineRenderOffTime);
            lineRender.enabled = false;
        }
    }
}