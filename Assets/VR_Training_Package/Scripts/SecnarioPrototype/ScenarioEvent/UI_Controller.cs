using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Controller : MonoBehaviour
{
    public static UI_Controller instance;

    int currentGoalCount;
    public Text textPatternType;
    public Text textTitle;
    public Text textDescription;
    public Text textPatternGoalCount;
    public Text textCurrentGoalCount;
    public Text textPatternTypeLarge;

    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }

    void Start()
    {
        
    }

    public void SetPatternType(string value)
    {
        textPatternType.text = "Pattern Type : " + value;
    }


    public void SetTitle(string value)
    {
        textTitle.text = "Pattern Title : " + value;
    }

    public void SetDescription(string value)
    {
        textDescription.text = "Pattern Description : " + value;
    }

    public void SetPaternGoalCount(int value)
    {
        textPatternGoalCount.text = "Pattern Goal Count : " +value.ToString();
    }

    public void SetCurrentCoalCount(int value)
    {
        textCurrentGoalCount.text = " Current Count : " + value.ToString();
        SetLargeText();
    }
    
    public void SetLargeText()
    {
        var type = textPatternType.text.Split('_')[1];
        var goalCount = textPatternGoalCount.text.Split(':')[1].Trim();
        var currentCount = textCurrentGoalCount.text.Split(':')[1].Trim();
        textPatternTypeLarge.text = $"{type}-{goalCount}/{currentCount}";  //value.Split('_')[1];

    }

}
