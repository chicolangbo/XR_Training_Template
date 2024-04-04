using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIButtonEvent : UIButton
{
    public UnityEvent unityEvent;
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(()=>ClickBtn_Maker(this));
    }

    public void ClickBtn_Maker(UIButton btn)
    {
        unityEvent?.Invoke();
        if(clickType != EnumDefinition.UIClickType.SideEvent)
            Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnUISelect, btn);
    }

    public void OverEnter()
    {
        
    }

    public void OverExit()
    {

    }
}
