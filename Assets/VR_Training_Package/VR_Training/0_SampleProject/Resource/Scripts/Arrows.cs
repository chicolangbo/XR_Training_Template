using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrows : MonoBehaviour
{

    public float dirY = 0.001f, speed = 2.5f;
    Vector3 pos2;

    void Update()
    {
        ArrowMove();
    }

    // �࿡ ���� �̵� �ݺ��ϱ�
    void ArrowMove()
    {
        pos2 = this.transform.position;
        pos2.y += dirY * Mathf.Sin(Time.time * speed);
        this.transform.position = pos2;
    }
}
