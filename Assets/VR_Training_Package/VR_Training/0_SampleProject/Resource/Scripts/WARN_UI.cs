using UnityEngine;

public class WARN_UI : MonoBehaviour
{
    private bool imageState = false;
    GameObject warn;
    void Start()
    {
        warn = GameObject.FindGameObjectWithTag("WARN");
    }
    void Update()
    {
        if (imageState == true)
        {
            ImageON();
        }
        else if (imageState == false)
        {
            ImageOFF();
        }
    }
    //호버 시 안내 문구 보이기
    public void ImageON()
    {
        imageState = true;
        warn.SetActive(true);
    }

    //호버 아닐 시 안내 문구 가리기
    public void ImageOFF()
    {
        imageState = false;
        warn.SetActive(false);
    }
}
