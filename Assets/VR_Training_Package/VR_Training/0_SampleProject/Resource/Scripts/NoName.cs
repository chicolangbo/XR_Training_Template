using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class NoName : MonoBehaviour
{
    //UnityAction action;
    Collision target;
    GameObject target2;

    public Transform testValue_A;
    public Transform testValue_B;
    Vector3 pointArea;
    Quaternion pointQuaternion;

    public float dirValue = 0.2f;
    public float dirFR_Sub;
    public bool isdirFR = false;

    void Awake()
    {
        FindTransform();
        target2 = GameObject.FindGameObjectWithTag("Handle");

    }

    void Update()
    {
        dirValueSet();
    }

    void OnCollisionEnter(Collision target)
    {
        SetCollision();
        HandleCollidierDestroy();
    }

    void HandleCollidierDestroy()
    {
        if (target2 && dirFR_Sub < dirValue)
        {
            Destroy(target2.GetComponent<XRGrabInteractable>());
            Destroy(target2.GetComponent<Rigidbody>());
        }
    }
    void SetCollision()
    {
        if (target2 && dirFR_Sub < dirValue)
        {
            Destroy(this.GetComponent<MeshCollider>());
            target2.transform.position = pointArea;
            target2.transform.rotation = pointQuaternion;
        }
    }

    // 방향 벡터 연산
    void dirValueSet()
    {
        Vector3 dirFR_A = testValue_A.rotation * (testValue_A.forward + testValue_A.right);
        Vector3 dirFR_B = testValue_B.rotation * (testValue_B.forward + testValue_B.right);
        dirFR_Sub = (dirFR_A - dirFR_B).sqrMagnitude;
    }

    // 태그 찾아 트랜스폼 자동 연결
    void FindTransform()
    {
        testValue_A = GameObject.FindGameObjectWithTag("Handle").transform;
        testValue_B = GameObject.FindGameObjectWithTag("HandlePoint").transform;

        //포지션, 쿼터니언 찾아서 전달 
        pointArea = GameObject.FindGameObjectWithTag("HandlePoint").transform.position;
        pointQuaternion = GameObject.FindGameObjectWithTag("HandlePoint").transform.rotation;
    }

}
