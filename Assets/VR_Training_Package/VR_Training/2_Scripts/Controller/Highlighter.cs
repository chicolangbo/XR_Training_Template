using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Highlighter : MonoBehaviour
{
    public List<Renderer> childReneders = new List<Renderer>();
    
    [HideInInspector]
    public bool isHighlightOn;

    public void SetHighlight(bool value)
    {
        for (int i = 0; i < childReneders.Count; i++)
        {
            if (childReneders[i] == null)
                continue;
            if(IsMultipleMats(childReneders[i]))
            {
                foreach(var mat in childReneders[i].materials)
                {
                    mat.SetFloat("Rim_intensity", value ? 1f : 0f);
                }
            }
            else
            {
                childReneders[i].material.SetFloat("Rim_intensity", value ? 1f : 0f);
            }

            
        }
        isHighlightOn = value;
    }

    bool IsMultipleMats(Renderer rd)
    {
        return rd.materials.Length >= 2;
    }


    public void HighlightOn()
    {
        // 평가 모드와 아닐때
        if (Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.EVALUATION)
        {
            if (Secnario_UserContext.instance.evalutionHighlightOn)
            {
                SetHighlight(true);
            }
            else
            {
                bool isDataHighlightOff = Secnario_UserContext.instance.IsEvalHighLightOff();
                SetHighlight(!isDataHighlightOff);
            }
        }
        else
        {
            SetHighlight(true);
        }
    }
    public void HighlightOff()
    {
        SetHighlight(false);
    }

    public void GetChildReneders()
    {
        //childReneders = UtilityMethod.GetAllChildComponets<Renderer>(transform).Where(w => w.gameObject.name != gameObject.name).ToList();
        childReneders = UtilityMethod.GetAllChildComponets<Renderer>(transform).ToList();
    }
}
