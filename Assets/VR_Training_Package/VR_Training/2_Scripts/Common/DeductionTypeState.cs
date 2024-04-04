using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeductionTypeState : MonoBehaviour
{
    public EnumDefinition.Deduction_Type deduction_Type;

    public float initTransformHeight = 42.1f;
    private List<string> evaluationItems = new List<string>();



    private void Start()
    {
        //StartCoroutine(InitHeight());
    }

    private IEnumerator GetInitHeight()
    {
        initTransformHeight = GetComponent<RectTransform>().sizeDelta.y;

        while (true)
        {
            yield return null;
            if(initTransformHeight != 0)
            {
                yield break;
            }
        }
    }
    public void AddEvaluationItem(string item)
    {
        if (evaluationItems.Contains(item) == false)
        {
            var deductionFactor = transform.Find("deductionFactor");
            var evaluationItem = transform.Find("evaluationItem");
            var score = transform.Find("score");
            if(evaluationItems.Count == 0)
            {
                evaluationItem.Find("Text").GetComponent<Text>().text += $"{item}";
            }
            else
            {
                evaluationItem.Find("Text").GetComponent<Text>().text += $"\n{item}";
            }
            SetTransformHeight(deductionFactor);
            SetTransformHeight(evaluationItem);
            SetTransformHeight(score);
            evaluationItems.Add(item);
        }
    }

    public int GetEvaluationItemCount()
    {
        return evaluationItems.Count;
    }

    private void SetTransformHeight(Transform target)
    {
        if (target != null)
        {
            target.GetComponent<RectTransform>().sizeDelta = new Vector2(target.GetComponent<RectTransform>().sizeDelta.x, initTransformHeight + initTransformHeight * GetEvaluationItemCount());
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)target.parent.transform);

        }
    }

    public void TestAddEvaluationItem(string value)
    {
        AddEvaluationItem(value);
    }
}
