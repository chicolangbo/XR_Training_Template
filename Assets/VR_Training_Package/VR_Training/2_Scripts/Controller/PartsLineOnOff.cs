using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsLineOnOff : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Line;
    public GameObject head;

    public bool isHide_Start = true;
    void Start()
    {
        if (isHide_Start)
        {
            if (Line != null)
                Line.SetActive(false);

            if (head != null)
                head.SetActive(false);

            else
                Debug.LogError("Line is Null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LineOn()
    {
        Debug.Log("LineON");
        if (Line != null)
            Line.SetActive(true);
        else
            Debug.LogError("Line is Null");
    }

    public void LineOff()
    {
        Debug.Log("LineOff");
        if (Line != null)
            Line.SetActive(false);
        else
            Debug.LogError("Line is Null");
    }

    public void HeadOn()
    {
        if (head != null)
            head.SetActive(true);
        else
            Debug.LogError("head is Null");
    }
}
