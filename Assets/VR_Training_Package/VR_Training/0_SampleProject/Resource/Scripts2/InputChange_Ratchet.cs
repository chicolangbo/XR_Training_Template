using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;

public class InputChange_Ratchet : MonoBehaviour
{
    public Transform ratchetSwitch;
    private void Update()
    {
        this.transform.position = ratchetSwitch.transform.position;
        this.transform.rotation = ratchetSwitch.transform.rotation;
    }

}
