using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class AnalysisUI : MonoBehaviour
{
    public Image image1, image2;

    private void Start()
    {
        image1.gameObject.SetActive(true);
        image2.gameObject.SetActive(false);
       
        Invoke("SetUI", 1);
    }

    void SetUI()
    {
        image2.gameObject.SetActive(true);
        image1.gameObject.SetActive(false); 
    }

}
