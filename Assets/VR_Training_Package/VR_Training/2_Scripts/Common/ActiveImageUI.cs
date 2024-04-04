using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveImageUI : MonoBehaviour
{
    public GameObject[] img;
    int num = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnImage()
    {
        if (img[num])
        {
            img[num].SetActive(true);
            num++;
        }
        else
        {
            Debug.LogError("이미지 없음");
        }
    }

    public void OffImage()
    {
        for(int i=0; i<img.Length; i++)
        {
            img[i].SetActive(false);
        }
    }
    public void DelayOnImage()
    {
        StartCoroutine(D_OnImage());
    }

    IEnumerator D_OnImage()
    {
        yield return new WaitForSeconds(2.0f);
        if (img[num])
        {
            img[num].SetActive(true);
            num++;
        }
        else
        {
            Debug.LogError("이미지 없음");
        }
    }
}
