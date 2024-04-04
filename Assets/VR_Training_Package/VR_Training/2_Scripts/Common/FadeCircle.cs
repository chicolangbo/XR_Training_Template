using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCircle : MonoBehaviour
{
    public Renderer render;
    float a;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if(Secnario_UserContext.instance.initialDone)
        {
            a += Time.deltaTime;
            render.material.SetColor("_TintColor", new Color(0, 0, 0, 1 - a));
            if (a >= 1)
            {
                Secnario_UserContext.instance.CameraUISetting();
                List<PartsID> tools = PartsTypeObjectData.instance.GetPartsIDByType(EnumDefinition.PartsType.TOOL);
                foreach (PartsID part in tools)
                {
                    if (part.transform.Find("size"))
                    {
                        part.transform.Find("size").gameObject.SetActive(true);
                    }
                }

                Secnario_UserContext.instance.initialDone = false;
                a = 0; 
            }
        }

    }

    public void FadeIn()
    {
        StartCoroutine(CoFadeIn());
    }

    IEnumerator CoFadeIn()
    {
        while(true)
        {
            yield return null;
            a += Time.deltaTime;
            render.material.SetColor("_TintColor", new Color(0, 0, 0, a));
            if (a >= 1)
            {
                a = 0; 
                break;
            }
              
        }
    }

    public void FadeOut()
    {
        StartCoroutine(CoFadeOut()); 
    }

    IEnumerator CoFadeOut()
    {
        while (true)
        {
            yield return null;
            a += Time.deltaTime;
            render.material.SetColor("_TintColor", new Color(0, 0, 0,1 - a));
            if (a >= 1)
            {
                a = 0;
                break;
            }

        }
    }

   


}
