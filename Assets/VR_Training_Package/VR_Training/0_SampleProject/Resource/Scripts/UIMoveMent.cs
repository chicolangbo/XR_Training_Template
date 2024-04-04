using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoveMent : MonoBehaviour
{

    private float dirY = 0.0005f, speed = 1.0f;
    Vector3 pos;

    void Update()
    {
        UIMove();
    }

    // �࿡ ���� �̵� �ݺ��ϱ�
    void UIMove()
    {
        pos = this.transform.position;
        pos.y += dirY * Mathf.Sin(Time.time * speed);
        this.transform.position = pos;
    }
}
