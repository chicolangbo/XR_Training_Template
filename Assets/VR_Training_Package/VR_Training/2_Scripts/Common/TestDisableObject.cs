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
        yield return new WaitForSeconds(delay); // 지정된 지연 시간만큼 대기
        gameObject.SetActive(false); // 게임 오브젝트를 비활성화
    }
}
