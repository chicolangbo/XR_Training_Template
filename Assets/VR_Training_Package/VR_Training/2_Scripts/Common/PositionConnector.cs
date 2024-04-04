using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PositionConnector : MonoBehaviour
{
    public Transform target;

    //public bool 
    void Start()
    {
        
    }

    
    void Update()
    {

        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
        
    }
}
