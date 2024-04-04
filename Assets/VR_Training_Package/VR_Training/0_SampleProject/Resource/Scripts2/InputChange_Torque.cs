using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;

public class InputChange_Torque : MonoBehaviour
{
    public Transform torqueSwitch;
    private void Update()
    {
        this.transform.position = torqueSwitch.transform.position;
        this.transform.rotation = torqueSwitch.transform.rotation;
    }

}
