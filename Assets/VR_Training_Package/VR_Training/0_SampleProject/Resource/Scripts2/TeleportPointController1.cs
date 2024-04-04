using UnityEngine;
using UnityEngine.XR;

public class TeleportPointController1 : MonoBehaviour
{
    public Transform teleportPoint;
    public GameObject controller;
    public GameObject reticleSet;
    public float moveSpeed = 0.04f, line_Height = 0.5f;
    public EnumDefinition.ControllerType controllerType;
    InputDevice cont;

    //test parts
    private float delayTime = 0f;
    private int num = 0;
    Vector3 pos;

    // Update is called once per frame

    void Start()
    {
        Init();
    }
    void Update()
    {
        if (XR_ControllerBase.instance.isControllerReady)
        {
            cont = XR_ControllerBase.instance.GetInputDeviceController(controllerType);
            controller.transform.position = new Vector3(GameObject.FindWithTag("MainCamera").transform.position.x, line_Height, GameObject.FindWithTag("MainCamera").transform.position.z);
            switch (num)
            {
                case 0:
                    GetDelay();
                    break;
                case 1:
                    UpdateTelepointPosition();
                    break;
                case 2:
                    break;
            }
        }
    }

    void Init()
    {
        teleportPoint.transform.position = new Vector3(GameObject.FindWithTag("XRrig").transform.position.x, GameObject.FindWithTag("XRrig").transform.position.y + 0.02f, GameObject.FindWithTag("XRrig").transform.position.z) + teleportPoint.transform.forward * 1.5f;
    }

    void GetDelay()
    {
        if (delayTime < 0.5f)
            delayTime += Time.deltaTime;
        if (delayTime >= 0.5f)
            num = 1;
    }
    void UpdateTelepointPosition()
    {
        // get pad point 
        if (cont.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 value))
        {
            teleportPoint.Translate(new Vector3(value.x, 0, value.y) * moveSpeed);
            //if (value.y > 0.1f || value.y < -0.1f || value.x > 0.1f || value.x < -0.1f)
            //teleportPoint.transform.rotation = Quaternion.Euler(GameObject.FindWithTag("MainCamera").transform.rotation.x, GameObject.FindWithTag("XRrig").transform.rotation.y, GameObject.FindWithTag("MainCamera").transform.rotation.z);
            if (value != Vector2.zero)
            {
                teleportPoint.Translate(new Vector3(value.x, 0, value.y) * moveSpeed);
                //if (value.y > 0.1f || value.y < -0.1f || value.x > 0.1f || value.x < -0.1f)
                reticleSet.SetActive(true);
                controller.SetActive(true);
            }
            else if (value == Vector2.zero)
            {
                pos = GameObject.FindWithTag("MainCamera").transform.rotation.eulerAngles;
                teleportPoint.transform.rotation = Quaternion.Euler(0, pos.y, 0);
                teleportPoint.transform.position = new Vector3(GameObject.FindWithTag("MainCamera").transform.position.x, GameObject.FindWithTag("XRrig").transform.position.y + 0.02f, GameObject.FindWithTag("MainCamera").transform.position.z) + teleportPoint.transform.forward * 1.5f;
                reticleSet.SetActive(false);
                controller.SetActive(false);
            }
            if (cont.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTrigger) && teleportPoint.GetComponent<Renderer>().material.color == Color.blue && value != Vector2.zero)
            {
                if (isTrigger)
                {
                    GameObject.FindWithTag("XRrig").transform.position = teleportPoint.transform.position;
                    teleportPoint.transform.position = new Vector3(GameObject.FindWithTag("MainCamera").transform.position.x, GameObject.FindWithTag("XRrig").transform.position.y + 0.02f, GameObject.FindWithTag("MainCamera").transform.position.z) + teleportPoint.transform.forward * 1.5f;

                    reticleSet.SetActive(false);
                    controller.SetActive(false);
                    num = 0;
                    delayTime = 0f;
                }
            }
        }
    }
}
