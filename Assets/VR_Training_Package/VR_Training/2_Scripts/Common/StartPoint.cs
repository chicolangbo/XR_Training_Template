using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction;

public class StartPoint : MonoBehaviour
{
    public Renderer my_rd;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetEnable());
    }

    IEnumerator SetEnable()
    {
        yield return new WaitForSeconds(1f);
       // my_rd.enabled = XR_ControllerBase.instance.isOnOculus ? false : true;
    }
 }
