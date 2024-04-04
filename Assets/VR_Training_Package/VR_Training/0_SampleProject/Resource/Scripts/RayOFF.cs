using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayOFF : MonoBehaviour
{
    public GameObject LeftRaycontroller;
    public GameObject RightRaycontroller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void _AllRayOff()
    {
        RightRaycontroller.SetActive(false);
        LeftRaycontroller.SetActive(false);
    }
    public void _AllRayOn()
    {
        RightRaycontroller.SetActive(true);
        LeftRaycontroller.SetActive(true);
    }
    public void _LeftRayOff()
    {
        LeftRaycontroller.SetActive(false);
    }
    public void _LeftRayOn()
    {
        LeftRaycontroller.SetActive(true);
    }
    public void _RightRayOff()
    {
        RightRaycontroller.SetActive(false);
    }
    public void _RighttRayOn()
    {
        RightRaycontroller.SetActive(true);
    }
}
