using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    
    void Update()
    {

        //var h = Mathf.Atan2(transform.forward.z, transform.forward.x) * Mathf.Rad2Deg;
        var x = transform.localEulerAngles.x;
        var y = transform.localEulerAngles.y;

        //if(h<0 && x > 270 && x < 360)
        //{
        //    calc = x - 540f;
        //}
        //else
        //{
        //    calc = x - 180f;  
        //}

        Debug.Log(GetAngleX(x,y));


    }


    float GetAngleX(float eulerAngleX , float eulerAngleY)
    {
        float calc;
        var harf = Mathf.Atan2(transform.forward.z, transform.forward.x) * Mathf.Rad2Deg;
        if (harf < 0 && eulerAngleX > 270 && eulerAngleX < 360)
            calc = eulerAngleX - (eulerAngleY*3);
        else
            calc = eulerAngleX - eulerAngleY;
        return Mathf.Abs(calc);
    }

}
