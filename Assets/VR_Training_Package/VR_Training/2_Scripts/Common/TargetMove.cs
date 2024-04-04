using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour
{
    public Pattern_051 p_051;

    Vector3 saveTransform;

    float time = 0;
    // Start is called before the first frame update
    bool isSucces = false;
    bool isTimeCheck = false;
    bool isCollider = false;
    void Start()
    {
        saveTransform = transform.localPosition;
    }

    public void TransformInit()
    {
        //transform.localPosition = saveTransform;
        isSucces = true;
        time = 0;
        isTimeCheck = true;
        Invoke("Check", 3.0f);
    }

    void Check()
    {
        isSucces = true;
    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A)){
        //    transform.localPosition += new Vector3(0.1f, 0f, 0f);
        //}
        //if (Input.GetKeyDown(KeyCode.S)){
        //    transform.localPosition -= new Vector3(0.1f, 0f, 0f);
        //}
        //Debug.Log(transform.localPosition);
        if (isTimeCheck)
        {
            if (!isCollider)
            {
                time = time + Time.deltaTime;
                Debug.LogError(time);
            }
            
        }
    }

    public void RightMove()
    {
        transform.localPosition += new Vector3(0.25f, 0f, 0f);
    }
    public void lefttMove()
    {
        transform.localPosition -= new Vector3(0.25f, 0f, 0f);
    }

    public bool IsSuccec()
    {


        isTimeCheck = false;
        if (time >= 5.0f) isSucces = false;
        else isSucces = true;

        Debug.Log("isSucces :   " + isSucces);
        if (p_051 != null)
            p_051.isSucces = isSucces;

        return isSucces;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("DriveCheck"))
        {
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "DriveCheck")
        {
            isCollider = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DriveCheck")
        {
            isCollider = true;
        }
    }

}
