using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandModelViewController : MonoBehaviour
{
    public EnumDefinition.ControllerType controllerType;
    public SkinnedMeshRenderer modelRD;

    void Start()
    {
       // transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }


    public void SetEnableModel(bool enableValue)
    {
        return; 
        modelRD.enabled = enableValue;
    }

}
