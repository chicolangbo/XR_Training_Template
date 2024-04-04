using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePivot : MonoBehaviour
{
    //다이렉트 인터렉터 찾은 후 그대로 포지션 회전값 가져오기
    void Update()
    {
        this.transform.position = GameObject.FindGameObjectWithTag("DR").transform.position;
        this.transform.rotation = GameObject.FindGameObjectWithTag("DR").transform.rotation;
    }
}
