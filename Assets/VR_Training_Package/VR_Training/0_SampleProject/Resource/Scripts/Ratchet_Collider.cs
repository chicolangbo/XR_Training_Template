using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ratchet_Collider : MonoBehaviour
{
    public bool colliderValue = false;


    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "bolt")
        {
            colliderValue = true;
        }
        

    }private void OnTriggerExit(Collider col)
    {
        if (col.tag == "bolt")
        {
            colliderValue = false;
        }

    }
}
