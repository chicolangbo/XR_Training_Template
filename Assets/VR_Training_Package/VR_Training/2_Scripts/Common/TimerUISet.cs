using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUISet : MonoBehaviour
{

    public Text txtTimeValue;
    void Start()
    {
        
    }

    public void SetTxtTimeValue(string txt)
    {
        txtTimeValue.text = txt;
    }

}
