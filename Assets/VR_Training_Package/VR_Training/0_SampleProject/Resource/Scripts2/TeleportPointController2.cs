using UnityEngine;
using UnityEngine.XR;

public class TeleportPointController2 : MonoBehaviour
{
    public Transform teleportPoint;
    public GameObject reticleSet;
    public GameObject controller;
    public float moveSpeed = 0.04f;
    public EnumDefinition.ControllerType controllerType;
    InputDevice cont;

    //test parts
    private float delayTime = 0f;
    private int num = 0;
    Vector3 pos;
    public Transform cam;

    //Color Test Parts
    public GameObject rayLine;
    public GameObject reticle;
    Renderer capsuleColor;
    Renderer liner;
    void Start()
    {

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

    void UpdateTelepointPosition()
    {
        // get pad point 
        if (cont.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 value))
        {

            var direction = cam.transform.forward;
            var rot = Quaternion.LookRotation(direction);
            var lookRot = Quaternion.Euler(teleportPoint.rotation.eulerAngles.x, rot.eulerAngles.y, teleportPoint.rotation.eulerAngles.z);
            reticleSet.transform.rotation = lookRot;

            if (value != Vector2.zero)
            {
                teleportPoint.Translate(new Vector3(value.x, 0, value.y) * moveSpeed);
                capsuleColor.material.color = Color.blue;
                liner.material.color = Color.blue;
                //if (value.y > 0.1f || value.y < -0.1f || value.x > 0.1f || value.x < -0.1f)
                reticleSet.SetActive(true);
            }
            else if (value == Vector2.zero)
            {
                pos = controller.transform.rotation.eulerAngles;
                teleportPoint.transform.rotation = Quaternion.Euler(0, pos.y, 0);
                teleportPoint.transform.position = new Vector3(controller.transform.position.x, GameObject.FindWithTag("XRrig").transform.position.y + 0.02f, controller.transform.position.z) + teleportPoint.transform.forward * 1.5f;
                reticleSet.SetActive(false);
                capsuleColor.material.color = new Color32(0, 0, 255, 122);
                liner.material.color = new Color32(0, 0, 255, 122);
            }

            if (cont.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTrigger) && teleportPoint.GetComponent<Renderer>().material.color == Color.blue && value != Vector2.zero)
            {
                if (isTrigger)
                {
                    GameObject.FindWithTag("XRrig").transform.position = teleportPoint.transform.position;
                    teleportPoint.transform.position = new Vector3(controller.transform.position.x, 0f, controller.transform.position.z) + teleportPoint.transform.forward * 1.5f;
                    reticleSet.SetActive(false);
                    num = 0;
                    delayTime = 0f;
                }
            }

            /*
            
            if (value == Vector2.zero)
            {
                pos = GameObject.FindWithTag("MainCamera").transform.rotation.eulerAngles;
                teleportPoint.transform.rotation = Quaternion.Euler(0, pos.y, 0);
                teleportPoint.transform.position = new Vector3(GameObject.FindWithTag("MainCamera").transform.position.x, 0.05f, GameObject.FindWithTag("MainCamera").transform.position.z) + teleportPoint.transform.forward * 1.5f;
                reticleSet.SetActive(false);
            }
            else if (value != Vector2.zero)
            {

                teleportPoint.Translate(new Vector3(value.x, 0, value.y) * moveSpeed);
                //if (value.y > 0.1f || value.y < -0.1f || value.x > 0.1f || value.x < -0.1f)
                    reticleSet.SetActive(true);

            }
            if (cont.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTrigger) && teleportPoint.GetComponent<Renderer>().material.color == Color.blue && value != Vector2.zero)
            {
                if (isTrigger)
                {
                    GameObject.FindWithTag("XRrig").transform.position = teleportPoint.transform.position;
                    timeValue = 0f;
                    reticleSet.SetActive(false);
                    num = 0;
                }
            }
            
            */
        }

    }

}