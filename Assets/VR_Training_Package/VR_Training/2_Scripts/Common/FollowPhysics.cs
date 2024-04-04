using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 패턴 11 휠타이어 흔들기에서 사용
/// </summary>
public class FollowPhysics : MonoBehaviour
{
    public Transform target;
    Rigidbody rb;
         

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.MovePosition(target.position);
    }
}
