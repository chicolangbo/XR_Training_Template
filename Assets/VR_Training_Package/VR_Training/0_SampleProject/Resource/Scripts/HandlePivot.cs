using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePivot : MonoBehaviour
{
    //���̷�Ʈ ���ͷ��� ã�� �� �״�� ������ ȸ���� ��������
    void Update()
    {
        this.transform.position = GameObject.FindGameObjectWithTag("DR").transform.position;
        this.transform.rotation = GameObject.FindGameObjectWithTag("DR").transform.rotation;
    }
}
