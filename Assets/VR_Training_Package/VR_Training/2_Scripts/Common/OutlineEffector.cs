using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;
public class OutlineEffector : MonoBehaviour
{

    public HighlightEffect highlightEffect;
    public bool isHightLightOn = false;
    public Collider outlineCollider;

    void Start()
    {
        if (TryGetComponent(out HighlightEffect _highlightEffect)) 
        {
            highlightEffect = _highlightEffect;
            
        }
        else
        {
            Debug.LogError(gameObject.name + " 게임 오브젝트에 HighlightEffect 컴포넌트가 존재 하지 않습니다.");
        }

        outlineCollider = gameObject.GetComponent<Collider>();
    }

    private void Update()
    {
        if(outlineCollider != null)
        {
            if(!outlineCollider.enabled)
            {
                OutlineOff();
                isHightLightOn = false;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RightController")
        {
            OutLineOn();
            isHightLightOn = true;
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (isHightLightOn)
        {
            OutlineOff();
            isHightLightOn = false;
        };
    }

    void OutLineOn()
    {
        highlightEffect.highlighted = true;
    }

    void OutlineOff()
    {
        highlightEffect.highlighted = false;
    }
         


}
