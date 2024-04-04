using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandleSet : MonoBehaviour
{
    //UnityAction action;
    GameObject target;

    public Transform testValue_A;
    public Transform testValue_B;
    Vector3 pointArea;
    Quaternion pointQuaternion;

    public float dirValue = 0.2f, rotValue1 = 0.0f, rotValue2 = 40.0f;
    public float dirFR_dir, dirFR_rot;
    public bool dirValueUse = true;

    void Awake()
    {
        //핸들과 핸들 부착점 받아오기
        FindTransform();
        target = GameObject.FindGameObjectWithTag("Handle");
    }

    void Update()
    {
        // 예외처리 (쿼터니언..)
        dirValueSet();
    }

    void OnCollisionEnter(Collision tar)
    {
        if (target && dirFR_dir < dirValue && rotValue1 < dirFR_rot  && dirFR_rot < rotValue2)
        {
            SetCollision();
            HandleCollidierDestroy();
        }
    }

    // 핸들 부착 시 조작이 불가능 하도록 그랩과 릿지드바디 파괴
    void HandleCollidierDestroy()
    {
        Destroy(target.GetComponent<XRGrabInteractable>());
        Destroy(target.GetComponent<Rigidbody>());
    }
    //범위에 해당 시 대상의 포지션과 회전값으로 변경 후 / 대상 파괴 및 부착
    void SetCollision()
    {
        Destroy(this.GetComponent<MeshCollider>());
        target.transform.position = pointArea;
        target.transform.rotation = pointQuaternion;
    }

    // 방향 벡터 연산
    void dirValueSet()
    {
        // 부울식 처리
        if (dirValueUse == true)
        {

            Vector3 dirFR_A = testValue_A.rotation * (testValue_A.forward + testValue_A.right);
            Vector3 dirFR_B = testValue_B.rotation * (testValue_B.forward + testValue_B.right);
            dirFR_dir = (dirFR_B - dirFR_A).sqrMagnitude;
            dirFR_rot = GetAngle(testValue_B.position, testValue_A.position);

        }
    }

    public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
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