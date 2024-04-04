using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton2 : MonoBehaviour
{
    public EnumDefinition.UIClickType clickType;
    public GameObject highlight;
    public int index;
    public bool getParent;


    public void Button_Active()
    {
        highlight.SetActive(true);
        this.GetComponent<UnityEngine.UI.Button>().enabled = true;
        Debug.Log("하이라이트 켜주기");
        if (getParent)
            this.transform.parent.localScale = new Vector3(1, 1, 1);
    }
}
