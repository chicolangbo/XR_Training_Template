using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool_AngleBase : MonoBehaviour
{
    public bool isRotateReady = false;
    public GameObject uiProgressSet;
    public Collider HingeGrab;
    public bool isFakeWrench = false;


    public void RoateON()
    {
        isRotateReady = true;
    }

    public void RoateOFF()
    {
        isRotateReady = false;
    }

    public void HingeGrabON()
    {
        HingeGrab.enabled = true;
    }

    public void HingeGrabOFF()
    {
        HingeGrab.enabled = false;
    }

    public void SetFakeWrench(bool value)
    {
        isFakeWrench = value;
    }

    public void EnableProgress(EnumDefinition.BoltProgressType progressType, PartsID parent = null)
    {
        if (progressType == EnumDefinition.BoltProgressType.Enable)
        {
            ProgressUiON(parent);
            isFakeWrench = false;
        }
        else
        {
            ProgressUiOFF(parent);
            isFakeWrench = true;
        }
    }

    public void ProgressUiON(PartsID parent = null)
    {
        uiProgressSet.SetActive(true);
        if(parent)
        {
            uiProgressSet.transform.SetParent(parent.transform);
        }
    }

    public void ProgressUiOFF(PartsID parent = null)
    {
        uiProgressSet.SetActive(false);
        if(parent)
        {
            uiProgressSet.transform.SetParent(transform);
        }
    }

    public abstract void SetToolDirection(EnumDefinition.ToolDirType toolDirType);

    public abstract void SetToolZRotaion(float zRotValue);

}
