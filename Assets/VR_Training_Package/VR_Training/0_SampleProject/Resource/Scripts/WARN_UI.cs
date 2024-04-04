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
    //ȣ�� �� �ȳ� ���� ���̱�
    public void ImageON()
    {
        imageState = true;
        warn.SetActive(true);
    }

    //ȣ�� �ƴ� �� �ȳ� ���� ������
    public void ImageOFF()
    {
        imageState = false;
        warn.SetActive(false);
    }
}
