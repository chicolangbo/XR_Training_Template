using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDisableObject : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DeactivateAfterDelay(8f));
    }

    IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ ���� �ð���ŭ ���
        gameObject.SetActive(false); // ���� ������Ʈ�� ��Ȱ��ȭ
    }
}
