using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OutLineEffect : MonoBehaviour
{

    public List<Renderer> childReneders = new List<Renderer>();
    public List<MatData> originalMats = new List<MatData>();
    public Material outlineMat;
    bool isChangeMat = false;


    public 



    void Start()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RightController")
        {
            SetOutlineMat();
            isChangeMat = true;
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (isChangeMat)
        {
            SetOriginalMat();
            isChangeMat = false;
        };
    }

    public void ChangeMat(bool value)
    {
        for (int i = 0; i < childReneders.Count; i++)
        {
            if (childReneders[i] == null)
                continue;
            if (IsMultipleMats(childReneders[i]))
            {
                foreach (var mat in childReneders[i].materials)
                {
                    mat.SetFloat("Rim_intensity", value ? 1f : 0f);
                }
            }
            else
            {
                childReneders[i].material.SetFloat("Rim_intensity", value ? 1f : 0f);
            }
        }
    }

    public void SetOutlineMat()
    {
        for (int i = 0; i < childReneders.Count; i++)
        {
            if (childReneders[i] == null)
                continue;
            if (IsMultipleMats(childReneders[i]))
            {
                childReneders[i].sharedMaterials = GetMats(childReneders[i].materials.Length);
            }
            else
            {
                childReneders[i].material = outlineMat;
            }
        }
    }

    Material[] GetMats(int count)
    {
        Material[] mats = new Material[count];
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = outlineMat;
        }
        return mats;
    }

    public void SetOriginalMat()
    {
        for (int i = 0; i < childReneders.Count; i++)
        {
            if (childReneders[i] == null)
                continue;
            if (IsMultipleMats(childReneders[i]))
            {
                childReneders[i].sharedMaterials = originalMats[i].mats.ToArray();
            }
            else
            {
                childReneders[i].material = originalMats[i].mat;
            }
        }
    }

    bool IsMultipleMats(Renderer rd)
    {
        return rd.materials.Length >= 2;
    }

    public void GetChildReneders()
    {
        childReneders = UtilityMethod.GetAllChildComponets<Renderer>(transform).ToList();

        // set original mats
        for (int i = 0; i < childReneders.Count; i++)
        {
            MatData data = new MatData();
            if (IsMultipleMats(childReneders[i]))
            {
                foreach (var mat in childReneders[i].materials)
                {
                    data.mats.Add(mat);
                }
                
            }
            else
            {
                data.mat = childReneders[i].material;
            }
            originalMats.Add(data);
        }
    }

}

[System.Serializable]
public class MatData
{
    public Material mat;
    public List<Material> mats = new List<Material>();
}