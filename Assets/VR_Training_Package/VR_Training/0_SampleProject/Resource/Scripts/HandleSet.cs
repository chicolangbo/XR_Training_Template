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
        //�ڵ�� �ڵ� ������ �޾ƿ���
        FindTransform();
        target = GameObject.FindGameObjectWithTag("Handle");
    }

    void Update()
    {
        // ����ó�� (���ʹϾ�..)
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

    // �ڵ� ���� �� ������ �Ұ��� �ϵ��� �׷��� ������ٵ� �ı�
    void HandleCollidierDestroy()
    {
        Destroy(target.GetComponent<XRGrabInteractable>());
        Destroy(target.GetComponent<Rigidbody>());
    }
    //������ �ش� �� ����� �����ǰ� ȸ�������� ���� �� / ��� �ı� �� ����
    void SetCollision()
    {
        Destroy(this.GetComponent<MeshCollider>());
        target.transform.position = pointArea;
        target.transform.rotation = pointQuaternion;
    }

    // ���� ���� ����
    void dirValueSet()
    {
        // �ο�� ó��
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

    // �±� ã�� Ʈ������ �ڵ� ����
    void FindTransform()
    {
        testValue_A = GameObject.FindGameObjectWithTag("Handle").transform;
        testValue_B = GameObject.FindGameObjectWithTag("HandlePoint").transform;

        //������, ���ʹϾ� ã�Ƽ� ���� 
        pointArea = GameObject.FindGameObjectWithTag("HandlePoint").transform.position;
        pointQuaternion = GameObject.FindGameObjectWithTag("HandlePoint").transform.rotation;
    }
}