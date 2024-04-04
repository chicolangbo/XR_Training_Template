using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket_Collider : MonoBehaviour
{
    public bool ratchetCollider = false;
    public bool boltCollider = false;


    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Ratchet")
        {
            ratchetCollider = true;
        }
        
        if (col.tag == "bolt")
        {
            boltCollider = true;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Ratchet")
        {
            ratchetCollider = false;
        }
        
        if (col.tag == "bolt")
        {
            boltCollider = false;
        }
    }
}
