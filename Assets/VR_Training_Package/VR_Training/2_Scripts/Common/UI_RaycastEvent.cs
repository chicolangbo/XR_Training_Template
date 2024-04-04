using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UI_RaycastEvent : MonoBehaviour
{
    RaycastHit hit;
    Stack<UI_KeyboardKeyColorChanger> colorChangerList = new Stack<UI_KeyboardKeyColorChanger>();
    UI_KeyboardKeyColorChanger colorChanger;
    void Start()
    {
        
    }

    void Update()
    {
        var mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.TryGetComponent<UI_KeyboardKeyColorChanger>(out UI_KeyboardKeyColorChanger _colorChanger))
            {
                colorChanger = _colorChanger;
                if (!colorChangerList.Contains(colorChanger))
                {
                    colorChangerList.Push(colorChanger);
                }
                if (Input.GetMouseButton(0))
                {
                    colorChanger.SetColorPress();
                }
                else
                {
                    colorChanger.SetColorStay();
                }
            }
        }
        else
        {
            if (colorChangerList.Count > 0)
                colorChangerList.Pop().SetColorExit();
        }
    }

}
