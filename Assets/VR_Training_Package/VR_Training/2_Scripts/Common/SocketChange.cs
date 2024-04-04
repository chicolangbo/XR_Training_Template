using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketChange : MonoBehaviour
{
    public Transform socket_12mm_pivot;
    public Transform socket_12mm_pivot_under;

    public void SelectEnteredGrab(int id)
    {
        var partid = GlobalData.instance.GetPartsID_Tool(id);
        partid.transform.GetComponent<XRGrabInteractable>().attachTransform = socket_12mm_pivot_under.transform;

    }

    public void SelectExitedGrab(int id)
    {
        var partid = GlobalData.instance.GetPartsID_Tool(id);
        partid.transform.GetComponent<XRGrabInteractable>().attachTransform = socket_12mm_pivot.transform;
    }
}
