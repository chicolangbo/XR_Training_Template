using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���� 11 ��Ÿ�̾� ���⿡�� ���
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
