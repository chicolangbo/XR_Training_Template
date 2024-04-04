using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{
    public bool isCollider = false;
    // Start is called before the first frame update
    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "DriverTarget")
        {
            isCollider = true;
        }
    }
}