using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPartsIdObject : MonoBehaviour
{
    public PartsID partsID;
    

    void Start()
    {
        UtilityMethod.GetMyPartsID( gameObject, ref partsID);
        AddPartsId();    
    }

    private void OnDestroy()
    {
        RemovePartsId();
    }


    void AddPartsId()
    {
        PartsTypeObjectData.instance.AddPartsID_Data(partsID);
    }

    void RemovePartsId()
    {
        PartsTypeObjectData.instance.RemovePartsID_Data(partsID);
    }
}
