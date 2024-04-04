using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Arrow : MonoBehaviour
{
    public Image Up;
    public Image Down;

    public Image left1, left2;
    public Image right1, right2;
    float min = -236;
    float max = -113;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void EnableArrowUp(bool enable)
    {
        Up.gameObject.SetActive(enable);
    }

    public void EnableArrowDown(bool enable)
    {
        Down.gameObject.SetActive(enable); 
    }

    public void UpdateSideArrows(float originZ,float maxZ,float moveZ)
    {
        float arrow_distance = Mathf.Abs(min - max);
        Vector2 pos = left1.rectTransform.anchoredPosition;
        pos.y = min + arrow_distance * moveZ / (maxZ - originZ); 
        left1.rectTransform.anchoredPosition = pos;
        left2.rectTransform.anchoredPosition = pos;
        right1.rectTransform.anchoredPosition = pos;
        right2.rectTransform.anchoredPosition = pos; 
    }

}
