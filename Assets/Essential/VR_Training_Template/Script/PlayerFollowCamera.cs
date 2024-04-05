using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    public float yPos = 0.5f;
    private Vector3 prevPos;

    void Update()
    {
        if(Camera.main.transform.position != prevPos)
        {
            var curPos = Camera.main.transform.position;
            curPos.y = yPos;
            transform.position = curPos;

            prevPos = curPos;
        }
    }
}
