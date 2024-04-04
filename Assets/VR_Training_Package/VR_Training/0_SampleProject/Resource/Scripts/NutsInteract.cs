using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NutsInteract : MonoBehaviour
{
    GameObject FindPipe, canvases, bolt;
    Transform nutamount;

    // Start is called before the first frame update
    public float checkTimer = 0;
    public float speed = 0.1f;
    public Image nutbar;
    public bool grabOn = false;
    public float amountValue;
    // Start is called before the first frame update

    void Start()
    {
        canvases = GameObject.FindGameObjectWithTag("NutCanvas");
        nutamount = GameObject.FindWithTag("Nutgage").transform;
        bolt = GameObject.FindGameObjectWithTag("Bolt");
       

    }

    // Update is called once per frame
    void Update()
    {
        if (grabOn == true)
        {
            GageNutOn();
        }
        else if (grabOn == false)
        {
            GageNutOff();
        }
    }
    public void GageNutOn()
    {
        grabOn = true;
        canvases.SetActive(true);
    }
    public void GageNutOff()
    {
        grabOn = false;
        canvases.SetActive(false);



    }

    void AmountValue()
    {
        float value = 360f * 3f;

        //mathf.Abs() ¾ç¼ö

        //value = (value , from_min from_max,  toMin, toMax); // remap

        amountValue = remap(bolt.transform.rotation.y, 0, value, 0, 1);
    }

    //public float remap() { }
    private float remap(float aValue, float oriMin, float oriMax, float newMin, float newMax)
    {
        float normal = Mathf.InverseLerp(oriMin, oriMax, aValue);
        float bValue = Mathf.Lerp(newMin, newMax, normal);
        return bValue;
    }
    public void NutGage()
    {
        if (checkTimer < 1)
        {
            checkTimer += speed;
            var smoothValue = Mathf.SmoothStep(0, 100, checkTimer);
            nutbar.fillAmount = smoothValue;
        }
    }
}
