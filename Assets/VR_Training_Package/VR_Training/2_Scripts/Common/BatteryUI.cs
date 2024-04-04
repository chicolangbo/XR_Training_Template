using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour
{
    public List<Image> images;

    public void SetImage(int index)
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].gameObject.SetActive(false);
        }

        images[index].gameObject.SetActive(true); 
    }
  
}
