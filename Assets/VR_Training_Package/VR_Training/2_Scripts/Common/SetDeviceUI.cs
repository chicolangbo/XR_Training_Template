using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDeviceUI : MonoBehaviour
{
    public GameObject[] UI;
    public GameObject[] UI2;
    public float oculusUIValue;
    public float viveUIValue;
    public float oculusUIValue2;
    public float viveUIValue2;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Init_", 10.0f);
    }
    public void Init_()
    {
        if (XR_ControllerBase.instance.isOnOculus)
        {
            if (UI.Length != 0)
            {
                for (int i = 0; i < UI.Length; i++)
                {
                    UI[i].transform.localPosition = new Vector3(UI[i].transform.localPosition.x, UI[i].transform.localPosition.y, oculusUIValue);
                }
            }
            if (UI2.Length != 0)
            {
                for (int i = 0; i < UI2.Length; i++)
                {
                    UI2[i].transform.localPosition = new Vector3(UI2[i].transform.localPosition.x, UI2[i].transform.localPosition.y, oculusUIValue2);
                }
            }
        }
        else
        {
            if (UI.Length != 0)
            {
                for (int i = 0; i < UI.Length; i++)
                {
                    UI[i].transform.localPosition = new Vector3(UI[i].transform.localPosition.x, UI[i].transform.localPosition.y, viveUIValue);
                }
            }
            if (UI2.Length != 0)
            {
                for (int i = 0; i < UI2.Length; i++)
                {
                    UI2[i].transform.localPosition = new Vector3(UI2[i].transform.localPosition.x, UI2[i].transform.localPosition.y, viveUIValue);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
