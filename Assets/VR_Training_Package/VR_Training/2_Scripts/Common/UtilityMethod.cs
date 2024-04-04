using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

public static class UtilityMethod 
{

    public static float RemapValue(float value, float from1, float to1, float from2, float to2)

    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    public static float Remap(float value, float in_min, float in_max, float out_min, float out_max)
    {
        return out_min + (value - in_min) * (out_max - in_min) / (in_max - out_min);
    }

    public static XRController GetController(EnumDefinition.ControllerType controllerType)
    {
        var obj = GameObject.FindGameObjectWithTag(controllerType.ToString());
        if (obj != null)
        {
            if (obj.TryGetComponent<XRController>(out XRController cont))
                return cont;
            else
            {
                Debug.LogError("XRController 컴포넌트가 없음");
                return null;
            }
        }
        else
        {
            Debug.LogError(controllerType.ToString() + " Tag 게임 오브젝트가 없음");
            return null;
        }
            
    }

    public static float GetAngleV3(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    public static float GetAngleV2(Vector2 start, Vector2 end)
    {
        Vector2 v2 = end - start;
        return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
    }

    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (type != other.GetType()) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }

    public static T[] GetAllChildComponets<T>(Transform parent)
    {
        return  parent.GetComponentsInChildren<T>();
    }

    public static void GetMyPartsID( GameObject my , ref PartsID partsID)
    {
        if(partsID == null)
        {
            if (my.TryGetComponent(out PartsID id))
            {
                partsID = id;
            }
            else
            {
                Debug.LogError( my.name + " 오브젝트는 part id  컴포넌트 가 존재 하지 않습니다. ");
            }
        }
    }

    public static string[] GetOptionData()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "options.txt");
        var file = File.ReadAllText(path);
        return file.Split(',');
    }

    public static float SignedAngleTo(this Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(a.x * b.y - a.y * b.x, a.x * b.x + a.y * b.y) * Mathf.Rad2Deg;
    }

    public static void EnableHighLightSlot(bool enable,Transform parent = null,Vector3 pos = default(Vector3), Vector3 size = default(Vector3), Vector3 rot = default(Vector3))
    {
        //볼트가 없는 슬랏에 볼트끼울때 하이라이트 
        PartsID part = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.ICON, 1);
        part.transform.SetParent(parent);
        part.transform.localPosition = pos;
        part.transform.localScale = size;
        part.transform.localEulerAngles = rot; 
        part.gameObject.SetActive(enable);
    }

}
