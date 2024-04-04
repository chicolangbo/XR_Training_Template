
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PressGrab : MonoBehaviour
{
    public Image fill;
    public float duration = 2;
    public float startTime;
    public float animValue;
    public bool okValue = false;

    //���α׷��� �ð��� ���� �� ����
    public IEnumerator SetProgress()
    {
        startTime = Time.time;
        while (animValue < 3f)
        {
            yield return null;
            animValue = (Time.time - startTime) / duration;
            var smoothValue = Mathf.SmoothStep(0, 1, animValue);
            fill.fillAmount = smoothValue;
            if (fill.fillAmount == 1)
            {
                okValue = true;
            }

        }
    }
}
