using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GuideDirection
{
    LEFT,
    TOP,
    RIGHT,
    BOTTOM
}

public class GuideArrow : MonoBehaviour
{
    public GameObject guideArrow;

    [SerializeField]
    GuideDirection guideDirection;

    //[SerializeField]
    //public float centerDistance = 0f;
    [HideInInspector]
    public float centerDistance = 0f;

    

    void Start()
    {   
        //quaternion = new Quaternion();
        //position = new Vector3();
    }

    public void SetDirection(int value)
    {
        guideArrow.SetActive(true);
        switch (value)
        {
            case 0: 
                guideDirection = GuideDirection.LEFT;                    
                guideArrow.transform.rotation = Quaternion.Euler(0, 0, -90);
                guideArrow.transform.position = new Vector3(transform.position.x - centerDistance, transform.position.y, transform.position.z);                
                break;
            case 1: 
                guideDirection = GuideDirection.TOP;                                
                guideArrow.transform.rotation = Quaternion.Euler(180, 0, 0);
                guideArrow.transform.position = new Vector3(transform.position.x, transform.position.y + centerDistance, transform.position.z);                
                break;
            case 2: 
                guideDirection = GuideDirection.RIGHT;                                
                guideArrow.transform.rotation = Quaternion.Euler(0, 0, 90);
                guideArrow.transform.position = new Vector3(transform.position.x + centerDistance, transform.position.y, transform.position.z);                
                break;
            case 3: 
                guideDirection = GuideDirection.BOTTOM;                                
                guideArrow.transform.rotation = Quaternion.Euler(0, 0, 0);
                guideArrow.transform.position = new Vector3(transform.position.x, transform.position.y - centerDistance, transform.position.z);                
                break;
        }       
    }

    public void SetCenterDistance(float v)
    {
        guideArrow.SetActive(true);
        centerDistance = v;
        switch (guideDirection)
        {
            case GuideDirection.LEFT:                
                
                guideArrow.transform.rotation = Quaternion.Euler(0, 0, -90);
                guideArrow.transform.position = new Vector3(transform.position.x - centerDistance, transform.position.y, transform.position.z);
                
                break;
            case GuideDirection.TOP:                
                
                guideArrow.transform.rotation = Quaternion.Euler(180, 0, 0);
                guideArrow.transform.position = new Vector3(transform.position.x, transform.position.y + centerDistance, transform.position.z);
                
                break;
            case GuideDirection.RIGHT:                                
                guideArrow.transform.rotation = Quaternion.Euler(0, 0, 90);
                guideArrow.transform.position = new Vector3(transform.position.x + centerDistance, transform.position.y, transform.position.z);                
                break;
            case GuideDirection.BOTTOM:                                
                guideArrow.transform.rotation = Quaternion.Euler(0, 0, 0);
                guideArrow.transform.position = new Vector3(transform.position.x, transform.position.y - centerDistance, transform.position.z);                
                break;
        }
    }

    public void GuideArrowOn()
    {
        guideArrow.SetActive(true);
        switch (guideDirection)
        {
            case GuideDirection.LEFT:                
                guideArrow.transform.rotation = Quaternion.Euler(0, 0, -90);
                guideArrow.transform.position = new Vector3(transform.position.x - centerDistance, transform.position.y, transform.position.z);                
                break;
            case GuideDirection.TOP:                
                guideArrow.transform.rotation = Quaternion.Euler(180, 0, 0);
                guideArrow.transform.position = new Vector3(transform.position.x, transform.position.y + centerDistance, transform.position.z);
                
                break;
            case GuideDirection.RIGHT:                
                guideArrow.transform.rotation = Quaternion.Euler(0, 0, 90);
                guideArrow.transform.position = new Vector3(transform.position.x + centerDistance, transform.position.y, transform.position.z);
                
                break;
            case GuideDirection.BOTTOM:                
                guideArrow.transform.rotation = Quaternion.Euler(0, 0, 0);
                guideArrow.transform.position = new Vector3(transform.position.x, transform.position.y - centerDistance, transform.position.z);
                
                break;
        }
    }

    public void GuideArrowOff()
    {
        guideArrow.SetActive(false);
    }
    public void GuideArrowOn_Editor()
    {
        guideArrow.SetActive(true);

    }

    public void GuideArrowOff_Editor()
    {
        guideArrow.SetActive(false);

    }

    public void GetGuideArrow()
    {
        GameObject obj = GameObject.Find("GuideArrow");
        if (obj)
            guideArrow = obj;
        else
            Debug.LogWarning("GuideArrow NULL, DRAG IN PREFAB");
    }
}
